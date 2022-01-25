using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject satan, questionDialogBox, thinkingBubble, ready1Prefab, ready2Prefab, ready3Prefab, readyThinkPrefab;
    public Transform readyDisplayPosition;
    public float thinkingTime = 10f, animationMultiplier = 1f;
    public int numberOfGoodWords = 1, numberOfOkWords = 1, numberOfBadWords = 1, goodWordValue = 1, okWordValue = 1, badWordValue = 1;
    [HideInInspector] public Question question;

    int level = 0, questionIndex = 0, currentReadyPhase = 0;

    void Awake()
    {
        if(instance != null) return;
        instance = this;
    }

    void Start()
    {
        NextLevel();
    }

    public void StartTalking(List<string> answer, int difficulty)
    {
        Debug.Log(string.Join(" ", answer));
    }

    public void DisplayNextSentence()
    {
        if(question.question.Count > questionIndex)
        {
            questionDialogBox.GetComponentInChildren<Text>().text = question.question[questionIndex++];
        }
        else
        {
            questionDialogBox.GetComponentInChildren<Button>().enabled = false;
            InstantiateReadyPhase(ready1Prefab);
        }
    }

    public void DisplayNextReadyPhase()
    {
        currentReadyPhase++;

        GameObject readyPrefab = null;
        switch(currentReadyPhase)
        {
        case 1:
            readyPrefab = ready2Prefab;
            break;
        case 2:
            readyPrefab = ready3Prefab;
            break;
        case 3:
            StartThinking(question);
            return;
        default:
            return;
        }

        InstantiateReadyPhase(readyPrefab);
    }

    void NextLevel()
    {
        // Level increase
        level++;
        IncreaseGamePace();

        // Set up
        questionIndex = 0;
        currentReadyPhase = 0;
        satan.SetActive(true);
        questionDialogBox.GetComponentInChildren<Button>().enabled = true;

        // Asking the character a question
        question = ThinkingManager.instance.StartNewQuestion();
        questionDialogBox.GetComponentInChildren<Text>().text = question.question[questionIndex++];
        questionDialogBox.SetActive(true);
    }

    void StartThinking(Question question)
    {
        InstantiateReadyPhase(readyThinkPrefab);

        // Thinking of an appropriate answer
        satan.SetActive(false);
        questionDialogBox.SetActive(false);
        thinkingBubble.SetActive(true);
        ThinkingManager.instance.StartThinking(question);
    }

    void IncreaseGamePace()
    {
        thinkingTime -= 1.5f;
        animationMultiplier += 0.1f;

        ThinkingManager.instance.thoughtsMinSpeed += 25;
        ThinkingManager.instance.thoughtsMaxSpeed += 25;
        ThinkingManager.instance.thoughtsExplosionSpeed += 50;
        ThinkingManager.instance.minVelocityCooldown -= 0.3f;
        ThinkingManager.instance.maxVelocityCooldown -= 0.3f;
    }

    void InstantiateReadyPhase(GameObject readyPhase)
    {
        var phase = Instantiate(readyPhase, readyDisplayPosition.position, Quaternion.identity, readyDisplayPosition);
        phase.GetComponent<Animator>().speed = animationMultiplier;
    }
}
