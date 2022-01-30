using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour
{
    public TMP_Text text;
    ScoreManager sm;

    public enum Tier { S, A, B, C, D, F }

    void Start()
    {
        sm = GameObject.Find("Game Manager").GetComponent<ScoreManager>();
        text.text = sm.successes + " " + sm.fails + "\n" + sm.totalScore + " " + CalculateScore();
    }

    private Tier CalculateScore()
    {
        if (sm.totalScore > 200)
            return Tier.S;
        else if (sm.totalScore > 175)
            return Tier.A;
        else if (sm.totalScore > 150)
            return Tier.B;
        else if (sm.totalScore > 125)
            return Tier.C;
        else if (sm.totalScore > 100)
            return Tier.D;
        else
            return Tier.F;
    }
}
