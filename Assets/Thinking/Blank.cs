using UnityEngine;
using UnityEngine.UI;

public class Blank : MonoBehaviour
{
    WordClass wordClass;
    Text word;

    void Awake()
    {
        word = GetComponentInChildren<Text>();
    }

    public string Word
    {
        set { word.text = value; }
    }

    public WordClass WordClass
    {
        get { return wordClass; }
        set
        {
            wordClass = value;
            GetComponent<Image>().color = QuestionsManager.instance.WordClassToColor(wordClass);
        }
    }
}
