using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ThinkingManager : MonoBehaviour
{
    public static ThinkingManager instance;

    public GameObject thoughts;
    public Image clockFill;
    public AnswerSlot answerSlotPrefab;
    public Word wordPrefab;
    public Transform thoughtsDragParent;
    public Transform[] answerSlotsPositions = new Transform[4];

    public int mentalClutters = 5;
    public float thoughtsMinSpeed = 75f, thoughtsMaxSpeed = 150f, thoughtsExplosionSpeed = 500f, minVelocityCooldown = 4f, maxVelocityCooldown = 6f;

    List<WordClass> wordClasses = new List<WordClass> { WordClass.Noun, WordClass.Verb, WordClass.Adverb, WordClass.Adjective };

    bool thinking;
    float thinkingTimer, thinkingCooldown;

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

        if(thinkingTimer < 0f && thinking)
        {
            Debug.Log("Thinking process over. Time to talk!");
        }
    }

    public QuestionData StartNewQuestion()
    {
        var index = Random.Range(0, Database.instance.allQuestions.Count);
        var question = Database.instance.allQuestions[index];

        return question;
    }

    public void StartThinking(QuestionData question)
    {
        // Answer slots
        var answerSlotsData = new List<AnswerSlotData>(Database.instance.allAnswerSlots);
        var wordClassesCopy = new List<WordClass>(wordClasses);
        var answerSlots = new List<AnswerSlot>();
        for(var i = 0; i < 4; i++)
        {
            var random = Random.Range(0, answerSlotsData.Count);
            var data = answerSlotsData[random];
            var answerSlot = Instantiate(answerSlotPrefab, answerSlotsPositions[i]);
            answerSlot.data = data;
            var shape = Instantiate(data.shadowSlot, answerSlot.transform);
            shape.transform.SetAsFirstSibling();
            answerSlot.transform.SetAsFirstSibling();

            random = Random.Range(0, wordClassesCopy.Count);
            var wordClass = wordClassesCopy[random];
            wordClassesCopy.Remove(wordClass);
            answerSlot.wordClass = wordClass;

            answerSlots.Add(answerSlot);
        }

        // Words
        foreach(var slot in answerSlots)
        {
            // Good word
            var goodWord = Instantiate(wordPrefab, thoughts.transform.position, Quaternion.identity, thoughts.transform);
            var possibleWords = question.goodWords.Where(x => x.wordClass == slot.wordClass).ToList();
            var random = Random.Range(0, possibleWords.Count);
            var wordData = possibleWords[random];

            var shape = Instantiate(slot.data.goodAnswerShape, goodWord.transform);
            shape.transform.SetAsFirstSibling();
            goodWord.word = wordData.word;
            goodWord.value = GameManager.instance.goodWordValue;
            goodWord.wordClass = slot.wordClass;

            // Ok words
            possibleWords = question.okWords.Where(x => x.wordClass == slot.wordClass).ToList();
            for(var i = 0; i < GameManager.instance.numberOfOkWords; i++)
            {
                if(possibleWords.Count == 0) break;

                var okWord = Instantiate(wordPrefab, thoughts.transform.position, Quaternion.identity, thoughts.transform);
                random = Random.Range(0, possibleWords.Count);
                wordData = possibleWords[random];
                possibleWords.Remove(wordData);

                random = Random.Range(0, slot.data.okAnswersShapes.Count());
                shape = Instantiate(slot.data.okAnswersShapes[random], okWord.transform);
                shape.transform.SetAsFirstSibling();
                okWord.word = wordData.word;
                okWord.value = GameManager.instance.okWordValue;
                okWord.wordClass = slot.wordClass;
            }

            // Bad words
            possibleWords = question.badWords.Where(x => x.wordClass == slot.wordClass).ToList();
            for(var i = 0; i < GameManager.instance.numberOfBadWords; i++)
            {
                if(possibleWords.Count == 0) break;
                
                var badWord = Instantiate(wordPrefab, thoughts.transform.position, Quaternion.identity, thoughts.transform);
                random = Random.Range(0, possibleWords.Count);
                wordData = possibleWords[random];
                possibleWords.Remove(wordData);

                random = Random.Range(0, slot.data.badAnswersShapes.Count());
                shape = Instantiate(slot.data.badAnswersShapes[random], badWord.transform);
                shape.transform.SetAsFirstSibling();
                badWord.word = wordData.word;
                badWord.value = GameManager.instance.badWordValue;
                badWord.wordClass = slot.wordClass;
            }
        }

        // Mental clutters
        var mentalCluttersCopy = new List<MentalClutter>(Database.instance.allMentalClutters);
        for(var i = 0; i < mentalClutters; i++)
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
}
