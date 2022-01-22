using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question")]
public class QuestionData : ScriptableObject
{
    public string question;
    public List<WordData> blankWords = new List<WordData>();
    public List<string> answer = new List<string>();
}
