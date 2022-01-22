using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestionsManager : MonoBehaviour
{
    public static QuestionsManager instance;

    public GameObject thoughts, answerPartPrefab, answerBlanksPrefab;
    public Blank blankPrefab;
    public Word wordPrefab;
    public Text questionText;
    public Transform thoughtsDragParent;
    public Transform[] answerSlots;
    public Color nounColor, verbColor, adjectiveColor, adverbColor;

    public int outOfContextWords = 3;

    private List<QuestionData> allQuestions;
    private List<WordData> allWords;

    void Awake()
    {
        if(instance != null) return;

        instance = this;
    }

    void Start()
    {
        LoadAllQuestions();
        LoadAllWords();
        StartNewQuestion();
    }

    public Color WordClassToColor(WordClass wordClass)
    {
        switch(wordClass)
        {
        case WordClass.Noun:
            return nounColor;
        case WordClass.Verb:
            return verbColor;
        case WordClass.Adjective:
            return adjectiveColor;
        case WordClass.Adverb:
            return adverbColor;
        default:
            return Color.white;
        }
    }

    void StartNewQuestion()
    {
        var index = Random.Range(0, allQuestions.Count);
        var question = allQuestions[index];

        // Actual question
        questionText.text = question.question;

        // Answer parts and blanks
        var nextAnswerSlot = 0;
        var lastWasBlank = false;
        GameObject currentSlot = null;
        for(var i = 0; i < question.answer.Count; i++)
        {
            var answerPart = question.answer[i];
            if(answerPart.StartsWith("("))
            {
                var wordClass = StringToWordClass(answerPart.Substring(1, answerPart.Length - 2));
                if(!lastWasBlank)
                {
                    currentSlot = Instantiate(answerBlanksPrefab, answerSlots[nextAnswerSlot]);
                    var blank = Instantiate(blankPrefab, currentSlot.transform);
                    blank.WordClass = wordClass;
                    lastWasBlank = true;
                    nextAnswerSlot++;
                }
                else if(currentSlot != null)
                {
                    var blank = Instantiate(blankPrefab, currentSlot.transform);
                    blank.WordClass = wordClass;
                }
            }
            else
            {
                currentSlot = Instantiate(answerPartPrefab, answerSlots[nextAnswerSlot]);
                currentSlot.GetComponent<Text>().text = answerPart;
                lastWasBlank = false;
                nextAnswerSlot++;
            }
        }

        // Words
        var outOfContextWords = allWords.Where(x => !question.blankWords.Any(y => y.word == x.word)).ToList();
        for(var i = 0; i < this.outOfContextWords; i++)
        {
            var random = Random.Range(0, outOfContextWords.Count);
            var outOfContextWord = outOfContextWords[random];
            outOfContextWords.Remove(outOfContextWord);

            var newWord = Instantiate(wordPrefab, RandomPositionWithinThoughts(), Quaternion.identity, thoughts.transform);
            newWord.transform.SetAsFirstSibling();
            newWord.WordValue = outOfContextWord.word;
            newWord.WordClass = outOfContextWord.wordClass;
        }

        foreach(var wordData in question.blankWords)
        {
            var newWord = Instantiate(wordPrefab, RandomPositionWithinThoughts(), Quaternion.identity, thoughts.transform);
            newWord.transform.SetAsFirstSibling();
            newWord.WordValue = wordData.word;
            newWord.WordClass = wordData.wordClass;
        }
    }

    WordClass StringToWordClass(string wordClassString)
    {
        switch(wordClassString)
        {
        case "noun":
            return WordClass.Noun;
        case "verb":
            return WordClass.Verb;
        case "adverb":
            return WordClass.Adverb;
        case "adjective":
            return WordClass.Adjective;
        default:
            return WordClass.Noun;
        }
    }

    Vector3 RandomPositionWithinThoughts()
    {
        var trt = thoughts.GetComponent<RectTransform>();
        var v = new Vector3[4];
        trt.GetWorldCorners(v);
        var maxX = v[2].x - 1f;
        var minX = v[0].x + 1f;
        var maxY = v[1].y - 1f;
        var minY = v[3].y + 1f;

        var x = Random.Range(minX, maxX);
        var y = Random.Range(minY, maxY);

        return new Vector3(x, y, thoughts.transform.position.z);
    }

	void LoadAllQuestions()
	{
		var guids = AssetDatabase.FindAssets("", new [] {"Assets/Questions"});
		
		var assetPathes = guids.Select(x => AssetDatabase.GUIDToAssetPath(x));
		allQuestions = assetPathes.Select(x => AssetDatabase.LoadAssetAtPath(x, typeof(QuestionData)) as QuestionData).ToList();
	}

    void LoadAllWords()
    {
        var guids = AssetDatabase.FindAssets("", new [] {"Assets/Words"});
		
		var assetPathes = guids.Select(x => AssetDatabase.GUIDToAssetPath(x));
		allWords = assetPathes.Select(x => AssetDatabase.LoadAssetAtPath(x, typeof(WordData)) as WordData).ToList();
    }
}
