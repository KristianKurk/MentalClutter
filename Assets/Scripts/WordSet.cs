using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word Set", menuName = "Word Set")]
public class WordSet : ScriptableObject
{
    public WordClass wordClass;
    public AnswerSlot answerSlot;
    public List<Word> goodWords;
    public List<Word> okWords;
    public List<Word> badWords;
}
