using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject mainCharacter, satan;
    public Vector3 beingAskedPosition, thinkingPosition, satanPosition;
    public Color thinkingBackgroundColor = Color.gray;
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

    }

    void PrepareNextLevel()
    {
        // Setting up satan and the main character
        satan.SetActive(true);
        mainCharacter.transform.position = beingAskedPosition;

        // Asking the character a question
        var question = QuestionsManager.instance.StartNewQuestion();
        var timeToThink = Time.time + questionReadingTime;

        while(timeToThink > Time.time);

        mainCharacter.transform.position = beingAskedPosition;
        QuestionsManager.instance.StartThinking(question);
    }
}
