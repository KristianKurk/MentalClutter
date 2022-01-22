using UnityEngine;
using UnityEngine.UI;

public class Blank : MonoBehaviour
{
    public WordClass wordClass;

    Text word;

    void Awake()
    {
        word = GetComponentInChildren<Text>();
    }

    public string text
    {
        set { word.text = value; }
    }
}
