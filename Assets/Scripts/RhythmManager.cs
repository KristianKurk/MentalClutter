using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour
{
    public static RhythmManager instance;

    public GameObject tilePrefab;
    public GameObject[] shutes;
    public KeyCode[] keyCodes;

    public int currentWordIndex = 0;

    public RhythmData[] rhythms;
    public int currentRhythm;

    public int[] beatToSpawn;
    public int currentBeat;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetNewSong(0);
    }

    public void SetNewSong(int rhythmIndex)
    {
        this.currentRhythm = rhythmIndex;
        currentWordIndex = 0;
        currentBeat = 0;

        for (int i = 0; i < rhythms[currentRhythm].sequence.Length; i++)
            rhythms[currentRhythm].sequence[i].beat += rhythms[currentRhythm].firstBeatOffset;

        MusicManager.instance.Init(rhythms[currentRhythm].clip, rhythms[currentRhythm].beatsPerMinute, rhythms[currentRhythm].beatsPerMinute, 4);
        CalculateBeatsToSpawn();
    }

    public void TempGoNext() {
        int nextSong = currentRhythm + 1;
        if (nextSong >= rhythms.Length)
            nextSong = 0;

        SetNewSong(nextSong);
    }

    public void NextBeat()
    {
        if (currentWordIndex < this.rhythms[currentRhythm].sequence.Length)
        {
            if (currentBeat == beatToSpawn[currentWordIndex])
            {
                //Randomly select a shute down which the tile will fall
                int randomShuteIndex = Random.Range(0, shutes.Length);
                GameObject randomShute = shutes[randomShuteIndex];

                GameObject newTile = Instantiate(tilePrefab, randomShute.transform);
                newTile.GetComponentInChildren<Text>().text = this.rhythms[currentRhythm].sequence[currentWordIndex].word;
                TileFall tileFall = newTile.GetComponent<TileFall>();
                tileFall.keyCode = keyCodes[randomShuteIndex];
                tileFall.beatToHit = rhythms[currentRhythm].sequence[currentWordIndex].beat;
                tileFall.secondsToFall = this.rhythms[currentRhythm].secondsToFall;
                tileFall.ySpawn = 353;
                tileFall.yEnd = -353;
                tileFall.targetCenter = -100;
                tileFall.beatMargin = 1;

                currentWordIndex++;
            }
        }
        currentBeat++;
    }

    private void CalculateBeatsToSpawn()
    {
        float beatsPerSec = MusicManager.instance.songBpm / 60;
        float beatsToFall = rhythms[currentRhythm].secondsToFall * beatsPerSec;
        beatToSpawn = new int[rhythms[currentRhythm].sequence.Length];
        for (int i = 0; i < rhythms[currentRhythm].sequence.Length; i++)
        {
            beatToSpawn[i] = rhythms[currentRhythm].sequence[i].beat - (int)beatsToFall;
        }
    }
}
