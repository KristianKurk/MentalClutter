using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Word : Thought
{
    public string word;
    public WordClass wordClass;

    Text text;

    protected override void Start()
    {
        base.Start();
        
        word = "color = word class";
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
            if(blank.wordClass == wordClass)
            {
                blank.text = word;
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
}
