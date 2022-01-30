using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public TMP_Text text;
    ScoreManager sm;
    public Button button;

    public enum Tier { S, A, B, C, D, F }

    void Start()
    {
        GameObject gm = GameObject.Find("Game Manager");
        if (gm)
        {
            sm = gm.GetComponent<ScoreManager>();
            button.onClick.AddListener(gm.GetComponent<GameManager>().GoToEasterEggScene);
        }

        text.text = CalculateScore().ToString();
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
