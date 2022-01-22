using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Word : Thought
{
    string word;
    WordClass wordClass;
    Text text;

    protected override void Start()
    {
        base.Start();
        
        text = GetComponentInChildren<Text>();
        text.text = word;
    }

    public override void OnPointerUp(PointerEventData eventData) 
    {
        dragging = false;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        var blank = results.Find(x => x.gameObject.GetComponent<Blank>()).gameObject?.GetComponent<Blank>();
        if(blank)
        {
            if(blank.WordClass == wordClass)
            {
                blank.Word = word;
                Destroy(gameObject);
            }
            else
            {
                // TODO Lower the player's health
                ReturnToPosition();
            }
        }
        else
        {
            ReturnToPosition();
        }
    }

    public string WordValue
    {
        set { word = value; }
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
