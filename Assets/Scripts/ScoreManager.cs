using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public int successes;
    public int fails;

    private void Awake()
    {
        instance = this;
    }

    public void IncrementSuccesses() {
        successes++;
    }

    public void IncrementFailures() {
        fails++;
    }
}
