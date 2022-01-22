using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Question")]
public class QuestionData : ScriptableObject
{
    public string question;
    public List<string> answer = new List<string>();
    public List<WordData> blankWords = new List<WordData>();
}
