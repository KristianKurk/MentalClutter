using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    public static Database instance;

    public List<Question> allQuestions;
    public List<MentalClutter> allMentalClutters;

    void Awake()
    {
        if(instance != null) return;
        instance = this;
    }
}
