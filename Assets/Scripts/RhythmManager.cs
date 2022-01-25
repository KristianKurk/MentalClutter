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

    public int[] beatToHit;
    public int[] beatToSpawn;
    public int currentBeat;

    public Word selectedNoun, selectedVerb, selectedAdjective, selectedAdverb;

    private void Awake()
    {
        instance = this;
    }

    public void SetNewSong(int rhythmIndex, Word selectedNoun = null, Word selectedVerb = null, Word selectedAdjective = null, Word selectedAdverb = null)
    {
        this.currentRhythm = rhythmIndex;
        currentWordIndex = 0;
        currentBeat = 0;

        this.selectedNoun = selectedNoun;
        this.selectedVerb = selectedVerb;
        this.selectedAdjective = selectedAdjective;
        this.selectedAdverb = selectedAdverb;

        beatToHit = new int[rhythms[currentRhythm].sequence.Length];
        for (int i = 0; i < rhythms[currentRhythm].sequence.Length; i++)
            beatToHit[i] = rhythms[currentRhythm].sequence[i].beat + rhythms[currentRhythm].beatOffset;

        MusicManager.instance.Init(rhythms[currentRhythm].clip, rhythms[currentRhythm].beatsPerMinute, rhythms[currentRhythm].beatsPerMinute, rhythms[currentRhythm].firstBeatOffset);
        CalculateBeatsToSpawn();
    }

    public void TempGoNext()
    {
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
                TileFall tileFall = newTile.GetComponent<TileFall>();
                tileFall.points = 1;
                SetText(tileFall);
                tileFall.keyCode = keyCodes[randomShuteIndex];
                tileFall.beatToHit = beatToHit[currentWordIndex];
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

    private void SetText(TileFall tile)
    {
        string word = this.rhythms[currentRhythm].sequence[currentWordIndex].word;

        if (word == "*noun*" && selectedNoun != null)
        {
            word = this.selectedNoun.word;
            tile.points = selectedNoun.value;
        }
        else if (word == "*noun*" && selectedVerb != null)
        {
            word = this.selectedVerb.word;
            tile.points = selectedVerb.value;
        }
        else if (word == "*noun*" && selectedAdjective != null)
        {
            word = this.selectedAdjective.word;
            tile.points = selectedAdjective.value;
        }
        else if (word == "*noun*" && selectedAdverb != null)
        {
            word = this.selectedAdverb.word;
            tile.points = selectedAdverb.value;
        }

        tile.gameObject.GetComponentInChildren<Text>().text = word;
    }

    private void CalculateBeatsToSpawn()
    {
        float beatsPerSec = MusicManager.instance.songBpm / 60;
        float beatsToFall = rhythms[currentRhythm].secondsToFall * beatsPerSec;
        beatToSpawn = new int[rhythms[currentRhythm].sequence.Length];
        for (int i = 0; i < rhythms[currentRhythm].sequence.Length; i++)
        {
            beatToSpawn[i] = beatToHit[i] - (int)beatsToFall;
        }
    }
}
