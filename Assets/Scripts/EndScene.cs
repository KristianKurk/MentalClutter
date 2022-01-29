using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScene : MonoBehaviour
{
    public TMP_Text text;
    ScoreManager sm;

    void Start()
    {
        sm = GameObject.Find("Game Manager").GetComponent<ScoreManager>();
        text.text = sm.successes + " " + sm.fails;
    }
}
