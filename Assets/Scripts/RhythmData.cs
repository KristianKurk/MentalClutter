using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Rhythm Data", menuName = "Rhythm Data")]
public class RhythmData : ScriptableObject
{
    [Serializable]
    public struct Sequence {
        public int beat;
        public string word;
    }
    public AudioClip clip;
    public int beatsPerMinute;
    public int beatsPerLoop;
    public int firstBeatOffset;
    public int beatOffset;
    public float secondsToFall;
    public Sequence[] sequence;
}
