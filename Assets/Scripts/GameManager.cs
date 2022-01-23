using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject mainCharacter, satan, questionDialogBox, thinkingBubble;
    [Min(1f)] public float questionReadingTime = 5f;
    public int numberOfOkWords = 1, numberOfBadWords = 1, goodWordValue = 1, okWordValue = 1, badWordValue = 1;

    int level = 1;

    void Awake()
    {
        if(instance != null) return;
        instance = this;
    }

    void Start()
    {
        QuestionsManager.instance.LoadAllAssets();
        PrepareNextLevel();
    }

    void PrepareNextLevel()
    {
        // Setting up satan and the main character
        satan.SetActive(true);
        mainCharacter.SetActive(true);

        // Asking the character a question
        var question = QuestionsManager.instance.StartNewQuestion();
        questionDialogBox.GetComponentInChildren<Text>().text = question.question;
        questionDialogBox.SetActive(true);

        // Time to read the question
        var timeToRead = Time.time + questionReadingTime;
        StartCoroutine(StartThinking(question, timeToRead));
    }

    IEnumerator StartThinking(QuestionData question, float timeToRead)
    {
        yield return new WaitForSeconds(timeToRead);

        // Thinking of an appropriate answer
        mainCharacter.SetActive(false);
        questionDialogBox.SetActive(false);
        thinkingBubble.SetActive(true);
        QuestionsManager.instance.StartThinking(question);
    }
}
