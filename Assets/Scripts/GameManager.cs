using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject mainCharacter, satan, questionDialogBox, thinkingBubble, ready1Prefab, ready2Prefab, ready3Prefab, readyThinkPrefab;
    public Transform readyDisplayPosition;
    [Min(1f)] public float thinkingTime = 10f;
    public int numberOfOkWords = 1, numberOfBadWords = 1, goodWordValue = 1, okWordValue = 1, badWordValue = 1;

    int level = 1, questionIndex, currentReadyPhase = 0;
    QuestionData question;

    void Awake()
    {
        if(instance != null) return;
        instance = this;
    }

    void Start()
    {
        PrepareNextLevel();
    }

    public void DisplayNextSentence()
    {
        if(question.question.Count > questionIndex)
            questionDialogBox.GetComponentInChildren<Text>().text = question.question[questionIndex++];
        else
        {
            questionDialogBox.GetComponent<Button>().enabled = false;
            Instantiate(ready1Prefab, readyDisplayPosition.position, Quaternion.identity, readyDisplayPosition);
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

        Instantiate(readyPrefab, readyDisplayPosition.position, Quaternion.identity, readyDisplayPosition);
    }

    void PrepareNextLevel()
    {
        // Set up
        questionIndex = 0;
        currentReadyPhase = 0;
        satan.SetActive(true);
        questionDialogBox.GetComponent<Button>().enabled = true;

        // Asking the character a question
        question = ThinkingManager.instance.StartNewQuestion();
        questionDialogBox.GetComponentInChildren<Text>().text = question.question[questionIndex++];
        questionDialogBox.SetActive(true);
    }

    void StartThinking(QuestionData question)
    {
        Instantiate(readyThinkPrefab, readyDisplayPosition.position, Quaternion.identity, readyDisplayPosition);

        // Thinking of an appropriate answer
        satan.SetActive(false);
        questionDialogBox.SetActive(false);
        thinkingBubble.SetActive(true);
        ThinkingManager.instance.StartThinking(question);
    }
}
