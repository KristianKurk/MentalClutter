using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int level = 0, numberOfGoodWords = 1, numberOfOkWords = 1, numberOfBadWords = 1, goodWordValue = 1, okWordValue = 1, badWordValue = 1;
    [HideInInspector] public Word noun, verb, adverb, adjective;
    [HideInInspector] public Question question;

    void Awake()
    {
        if(instance != null) return;
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

        if(!(SceneManager.GetActiveScene().name == "ThinkingTesting"))
            SceneManager.LoadScene("ThinkingTesting");

        // Level increase
        level++;
        InterviewManager.instance.IncreasePace();

        // Reset
        noun = null;
        verb = null;
        adverb = null;
        adjective = null;

        // Interview setup
        InterviewManager.instance.SetUp();
    }

    public void StartTalking(Question question, Word noun, Word verb, Word adverb, Word adjective)
    {
        this.question = question;
        this.noun = noun;
        this.verb = verb;
        this.adverb = adverb;
        this.adjective = adjective;

        SceneManager.LoadScene("RhythmTest");
    }
}
