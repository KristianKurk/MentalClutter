using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level = 0, numberOfGoodWords = 1, numberOfOkWords = 1, numberOfBadWords = 1, goodWordValue = 1, okWordValue = 1, badWordValue = 1;
    public string[] words;
    public int[] values;
    [HideInInspector] public Question question;

    void Awake()
    {
        if (instance != null) return;
        instance = this;

        GameObject.DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(NextLevel(0f));
    }

    public IEnumerator NextLevel(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!(SceneManager.GetActiveScene().name == "ThinkingTesting"))
            SceneManager.LoadScene("ThinkingTesting");

        // Level increase
        level++;
        InterviewManager.instance.IncreasePace();

        // Reset
        words = new string[4];
        values = new int[4];

        if (level == 6)
            SceneManager.LoadScene("EndScene");
        else
            InterviewManager.instance.SetUp();        // Interview setup
    }

    public void StartTalking(Question question, Word noun, Word verb, Word adverb, Word adjective)
    {
        this.question = question;

        if (noun != null)
        {
            words[0] = noun.word;
            values[0] = noun.value;
        }
        else {
            words[0] = string.Empty;
            values[0] = -10;
        }

        if (verb != null)
        {
            words[1] = verb.word;
            values[1] = verb.value;
        }
        else
        {
            words[1] = string.Empty;
            values[1] = -10;
        }

        if (adjective != null)
        {
            words[2] = adjective.word;
            values[2] = adjective.value;
        }
        else
        {
            words[2] = string.Empty;
            values[2] = -10;
        }

        if (adverb != null)
        {
            words[3] = adverb.word;
            values[3] = adverb.value;
        }
        else
        {
            words[3] = string.Empty;
            values[3] = -10;
        }


        SceneManager.LoadScene("RhythmTest");
    }
}
