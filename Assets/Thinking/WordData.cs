using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Word")]
public class WordData : ScriptableObject
{
    public string word;
    public WordClass wordClass;
}

public enum WordClass { noun, verb, adverb, adjective }
