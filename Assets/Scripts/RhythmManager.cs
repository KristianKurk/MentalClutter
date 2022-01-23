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

    public string[] currentSentence;
    public int currentWordIndex = 0;
    public int currentDifficulty; //to be implemented

    public int[] beatToHit;
    public int[] beatToSpawn;
    public float secondsToFall;
    public int currentBeat;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentWordIndex = 0;
        beatToSpawn = new int[beatToHit.Length];
        CalculateBeatsToSpawn();
    }

    public void SetCurrentSentence(string sentence)
    {
        currentSentence = sentence.Split(' ');
        currentWordIndex = 0;
        currentBeat = 0;
    }

    public void SetNewSong(AudioClip clip, int beatsPerMinute, int beatsPerLoop, int[] beatToHit, float secondsToFall, string sentence)
    {
        SetCurrentSentence(sentence);
        MusicManager.instance.Init(clip, beatsPerMinute, beatsPerLoop);
        this.beatToHit = beatToHit;
        this.secondsToFall = secondsToFall;
        CalculateBeatsToSpawn();
    }

    public void NextBeat()
    {
        if (currentWordIndex < currentSentence.Length)
        {
            if (currentBeat == beatToSpawn[currentWordIndex])
            {
                //Randomly select a shute down which the tile will fall
                int randomShuteIndex = Random.Range(0, shutes.Length);
                GameObject randomShute = shutes[randomShuteIndex];

                GameObject newTile = Instantiate(tilePrefab, randomShute.transform);
                newTile.GetComponentInChildren<Text>().text = currentSentence[currentWordIndex];
                newTile.GetComponent<TileFall>().keyCode = keyCodes[randomShuteIndex];
                newTile.GetComponent<TileFall>().beatToHit = beatToHit[currentWordIndex];
                newTile.GetComponent<TileFall>().secondsToFall = this.secondsToFall;

                currentWordIndex++;
            }
        }
        currentBeat++;
    }

    private void CalculateBeatsToSpawn()
    {
        float beatsPerSec = MusicManager.instance.songBpm / 60;
        float beatsToFall = secondsToFall * beatsPerSec;
        for (int i = 0; i < beatToHit.Length; i++)
        {
            beatToSpawn[i] = beatToHit[i] - (int)beatsToFall;
        }
    }
}
