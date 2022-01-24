using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    //Basic Parameters
    public float songBpm;
    public float secPerBeat;
    public float songPosition;
    public float songPositionInBeats;
    public float dspSongTime;
    public float firstBeatOffset;
    public AudioSource musicSource;

    //Looping Parameters
    public float beatsPerLoop;
    public int completedLoops;
    public float loopPositionInBeats;

    public float loopPositionInAnalog;
    private int previousWholeBeat;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);
        songPositionInBeats = songPosition / secPerBeat;

        if (previousWholeBeat < (int)songPositionInBeats && songPositionInBeats > 0)
        {
            RhythmManager.instance.NextBeat();
        }

        previousWholeBeat = (int)songPositionInBeats;

        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
            completedLoops++;
        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;

        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;
    }

    public void Init()
    {
        musicSource = GetComponent<AudioSource>();
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }
    public void Init(AudioClip clip, float songBpm, float beatsPerLoop, float firstBeatOffset)
    {
        this.songBpm = songBpm;
        this.beatsPerLoop = beatsPerLoop;
        this.firstBeatOffset = firstBeatOffset;
        musicSource = GetComponent<AudioSource>();
        musicSource.clip = clip;
        secPerBeat = 60f / songBpm;
        dspSongTime = (float)AudioSettings.dspTime;
        musicSource.Play();
    }
}
