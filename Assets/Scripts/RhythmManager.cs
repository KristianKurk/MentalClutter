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

    public int[] beatToDrop;
    public int currentBeat;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentWordIndex = 0;
    }

    public void SetCurrentSentence(string sentence)
    {
        currentSentence = sentence.Split(' ');
        currentWordIndex = 0;
        currentBeat = 0;
    }

    public void NextBeat()
    {
        if (currentWordIndex < currentSentence.Length)
        {
            if (currentBeat == beatToDrop[currentWordIndex])
            {
                //Randomly select a shute down which the tile will fall
                int randomShuteIndex = Random.Range(0, shutes.Length);
                GameObject randomShute = shutes[randomShuteIndex];

                GameObject newTile = Instantiate(tilePrefab, randomShute.transform);
                newTile.GetComponentInChildren<Text>().text = currentSentence[currentWordIndex];
                newTile.GetComponent<TileFall>().keyCode = keyCodes[randomShuteIndex];
                currentWordIndex++;
            }
        }

        currentBeat++;
    }
}
