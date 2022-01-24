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
public class Question : ScriptableObject
{
    public List<string> question;
    public List<AnswerThresholdPair> answers = new List<AnswerThresholdPair>();
    public List<WordSet> wordSets = new List<WordSet>();
}
