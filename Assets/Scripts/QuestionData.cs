using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnswerThresholdPair
{
    public string answer;
    public int threshold;
}

[CreateAssetMenu(fileName = "Question", menuName = "Question")]
public class QuestionData : ScriptableObject
{
    public List<string> question;
    public List<AnswerThresholdPair> answers = new List<AnswerThresholdPair>();
    public List<WordData> goodWords = new List<WordData>();
    public List<WordData> okWords = new List<WordData>();
    public List<WordData> badWords = new List<WordData>();
}
