using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ThinkingManager : MonoBehaviour
{
    public static ThinkingManager instance;

    public GameObject thoughts;
    public Image clockFill;
    public Transform thoughtsDragParent;
    public Transform[] answerSlotsPositions = new Transform[4];

    public int mentalClutters = 5;
    public float thoughtsMinSpeed = 200f, thoughtsMaxSpeed = 400f, thoughtsExplosionSpeed = 1000f, minVelocityCooldown = 4f, maxVelocityCooldown = 6f;

    bool thinking;
    float thinkingTimer, thinkingCooldown;
    List<AnswerSlot> answerSlots;

    void Awake()
    {
        if(instance != null) return;
        instance = this;
    }

    void Update()
    {
        if(thinking)
        {
            thinkingTimer -= Time.deltaTime;
            clockFill.fillAmount = Mathf.Max(0f, thinkingTimer / GameManager.instance.thinkingTime);
        }

        if(thinkingTimer < 0f && thinking || answerSlots != null && answerSlots.Count != 0 && answerSlots.All(x => x.disabled))
        {
            SendAnswer();
        }
    }

    public Question StartNewQuestion()
    {
        var index = Random.Range(0, Database.instance.allQuestions.Count);
        var question = Database.instance.allQuestions[index];

        return question;
    }

    public void StartThinking(Question question)
    {
        answerSlots = new List<AnswerSlot>();
        var wordSets = new List<WordSet>(question.wordSets);
        for(var i = 0; i < 4; i++)
        {
            // Answer slots
            var random = Random.Range(0, wordSets.Count);
            var wordSet = wordSets[random];
            wordSets.Remove(wordSet);

            var answerSlot = Instantiate(wordSet.answerSlot, answerSlotsPositions[i]);
            answerSlot.index = i;
            answerSlot.transform.SetAsFirstSibling();
            answerSlots.Add(answerSlot);

            // Good words
            var possibleWords = new List<Word>(wordSet.goodWords);
            for(var j = 0; j < GameManager.instance.numberOfGoodWords; j++)
            {
                if(possibleWords.Count == 0) break;

                random = Random.Range(0, possibleWords.Count);
                var goodWord = possibleWords[random];
                goodWord = Instantiate(goodWord, thoughts.transform.position, Quaternion.identity, thoughts.transform);
                possibleWords.Remove(goodWord);

                goodWord.value = GameManager.instance.goodWordValue;
                goodWord.index = i;
            }

            // Ok words
            possibleWords = new List<Word>(wordSet.okWords);
            for(var j = 0; j < GameManager.instance.numberOfOkWords; j++)
            {
                if(possibleWords.Count == 0) break;

                random = Random.Range(0, possibleWords.Count);
                var okWord = possibleWords[random];
                okWord = Instantiate(okWord, thoughts.transform.position, Quaternion.identity, thoughts.transform);
                possibleWords.Remove(okWord);

                okWord.value = GameManager.instance.okWordValue;
                okWord.index = i;
            }

            // Bad words
            possibleWords = new List<Word>(wordSet.badWords);
            for(var j = 0; j < GameManager.instance.numberOfBadWords; j++)
            {
                if(possibleWords.Count == 0) break;

                random = Random.Range(0, possibleWords.Count);
                var badWord = possibleWords[random];
                badWord = Instantiate(badWord, thoughts.transform.position, Quaternion.identity, thoughts.transform);
                possibleWords.Remove(badWord);

                badWord.value = GameManager.instance.badWordValue;
                badWord.index = i;
            }
        }

        // Mental clutters
        var mentalCluttersCopy = new List<MentalClutter>(Database.instance.allMentalClutters);
        for(var j = 0; j < mentalClutters; j++)
        {
            var random = Random.Range(0, mentalCluttersCopy.Count);
            var mentalClutter = mentalCluttersCopy[random];
            //mentalCluttersCopy.Remove(mentalClutter);

            var newWord = Instantiate(mentalClutter, thoughts.transform.position, Quaternion.identity, thoughts.transform);
            newWord.transform.SetAsLastSibling();
        }

        thinkingTimer = GameManager.instance.thinkingTime;
        thinking = true;
    }


    void SendAnswer()
    {
        var noun = answerSlots.Find(x => x.word?.wordClass == WordClass.Noun)?.word;
        var verb = answerSlots.Find(x => x.word?.wordClass == WordClass.Verb)?.word;
        var adverb = answerSlots.Find(x => x.word?.wordClass == WordClass.Adverb)?.word;
        var adjective = answerSlots.Find(x => x.word?.wordClass == WordClass.Adjective)?.word;

        ResetParameters();
        GameManager.instance.StartTalking(noun, verb, adjective, adverb);
    }

    void ResetParameters()
    {
        answerSlots = null;
        thinking = false;
    }

    // void SendAnswer()
    // {
    //     answerSlots = answerSlots.Where(x => x.word != null).ToList();
    //     var totalScore = answerSlots.Sum(x => x.word.value);
    //     totalScore += (4 - answerSlots.Count) * 5;

    //     foreach(var answer in GameManager.instance.question.answers)
    //     {
    //         if(totalScore > answer.threshold) continue;
            
    //         var answerList = answer.answer.Split(' ').Select(x => x.Trim()).ToList();
    //         for (int i = 0; i < answerList.Count; i++)
    //         {
    //             var word = answerList[i];
    //             if (!word.StartsWith("(")) continue;

    //             var wordClass = StringToWordClass(word.Substring(1, word.Length - 2));
    //             var answerSlotsOfWordClass = answerSlots.Where(x => x.word.wordClass == wordClass).ToList();

    //             if(answerSlotsOfWordClass.Count == 0)
    //             {
    //                 var badWords = GameManager.instance.question.wordSets.Find(x => x.wordClass == wordClass).badWords; // TODO Take from worst words new category?
    //                 var random = Random.Range(0, badWords.Count);
    //                 var badWord = badWords[random];

    //                 answerList[i] = badWord.word;
    //             }
    //             else
    //             {
    //                 var random = Random.Range(0, answerSlotsOfWordClass.Count);
    //                 var answerSlot = answerSlotsOfWordClass[random];

    //                 answerSlots.Remove(answerSlot);
    //                 answerList[i] = answerSlot.word.word;
    //             }

    //             if(i != 0)
    //                 answerList[i] = answerList[i].ToLower();
    //         }

    //         GameManager.instance.StartTalking(answerList, totalScore); // Make a function to calculate the difficulty with the total score
    //         break;
    //     }

    //     ResetParameters();
    // }

    // WordClass StringToWordClass(string wordClassString)
    // {
    //     switch(wordClassString.ToLower())
    //     {
    //     case "noun":
    //         return WordClass.Noun;
    //     case "verb":
    //         return WordClass.Verb;
    //     case "adverb":
    //         return WordClass.Adverb;
    //     case "adjective":
    //         return WordClass.Adjective;
    //     default:
    //         return WordClass.Noun;
    //     }
    // }
}
