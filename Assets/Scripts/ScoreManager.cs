using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int successes;
    public int fails;

    public Text successText;
    public Text failureText;

    private void Awake()
    {
        instance = this;
    }

    public void IncrementSuccesses() {
        successes++;
        successText.text = "Successes: " + successes;
    }

    public void IncrementFailures() {
        fails++;
        failureText.text = "Failures: " + fails;
    }
}
