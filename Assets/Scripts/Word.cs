using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Word : Thought
{
    public string word;
    public WordClass wordClass;
    public int value;

    Text text;

    protected override void Start()
    {
        base.Start();
        
        text = GetComponentInChildren<Text>();
        text.text = word;
    }

    public override void OnPointerUp(PointerEventData eventData) 
    {
        if(disabled) return;

        dragging = false;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        var answerSlot = results.Find(x => x.gameObject.GetComponentInParent<AnswerSlot>()).gameObject?.GetComponentInParent<AnswerSlot>();
        if(answerSlot)
        {
            if(answerSlot.wordClass == wordClass && !answerSlot.disabled)
            {
                disabled = true;
                answerSlot.value = value;
                answerSlot.disabled = true;
                transform.SetParent(answerSlot.transform);
                transform.SetAsLastSibling();
                transform.position = answerSlot.transform.position;
            }
            else
            {
                ReturnToPosition();
            }
        }
        else
        {
            ReturnToPosition();
        }
    }
}
