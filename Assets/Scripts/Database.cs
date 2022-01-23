using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database instance;

    public List<QuestionData> allQuestions;
    public List<MentalClutter> allMentalClutters;
    public List<AnswerSlotData> allAnswerSlots;

    void Awake()
    {
        if(instance != null) return;
        instance = this;
    }
}
