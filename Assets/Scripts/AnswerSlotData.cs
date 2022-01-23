using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Answer Slot", menuName = "Answer Slot")]
public class AnswerSlotData : ScriptableObject
{
    public GameObject shadowSlot;
    public GameObject goodAnswerShape;
    public List<GameObject> okAnswersShapes;
    public List<GameObject> badAnswersShapes;
}
