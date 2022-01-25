using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rhythm Data", menuName = "Rhythm Data")]
public class RhythmData : ScriptableObject
{
    public AudioClip clip;
    public int beatsPerMinute;
    public int beatsPerLoop;
    public int[] beatsToHit;
    public int firstBeatOffset;
    public float secondsToFall;
    public string[] words;
}
