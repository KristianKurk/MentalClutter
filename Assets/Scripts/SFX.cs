using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX instance;
    AudioSource source;

    public AudioClip TileSuccessSFX;
    public AudioClip HitFailSFX;

    public AudioClip[] missedNoteSounds;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
    }

    public void PlayTileSuccessSFX()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = TileSuccessSFX;
        source.Play();
    }

    public void PlayMissedNoteSFX(int index) {
        Debug.Log("pog");
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = missedNoteSounds[index];
        source.Play();
    }

    public void PlayWrongPressSFX()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = HitFailSFX;
        source.Play();
    }
}
