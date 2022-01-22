using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MentalClutter : Thought
{
    public override void OnPointerUp(PointerEventData eventData) 
    {
        dragging = false;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        var blank = results.Find(x => x.gameObject.GetComponent<Blank>()).gameObject?.GetComponent<Blank>();
        if(blank)
        {
            
            // TODO Lower the player's health
            Destroy(gameObject);
        }
        else
        {
            ReturnToPosition();
        }
    }
}
