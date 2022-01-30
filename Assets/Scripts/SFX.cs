using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX instance;
    public AudioSource source;

    public AudioClip TileSuccessSFX;
    public AudioClip HitFailSFX;

    public AudioClip badWordPlaced;
    public AudioClip okWordPlaced;
    public AudioClip goodWordPlaced;

    public AudioClip goodWordHit;
    public AudioClip okWordHit;
    public AudioClip badWordHit;

    public AudioClip[] missedNoteSounds;

    private void Awake()
    {
        instance = this;
       // source = GetComponent<AudioSource>();
    }

    public void PlayTileSuccessSFX()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = TileSuccessSFX;
        source.Play();
    }

    public void PlayMissedNoteSFX(int index) {
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

    public void PlayGoodWordPlacedSFX()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = goodWordPlaced;
        source.Play();
    }

    public void PlayOkWordPlacedSFX()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = okWordPlaced;
        source.Play();
    }

    public void PlayBadWordPlacedSFX()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = badWordPlaced;
        source.Play();
    }

    public void PlayGoodWordHit()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = goodWordHit;
        source.Play();
    }

    public void PlayOKWordHit()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = okWordHit;
        source.Play();
    }

    public void PlayBadWordHit()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = badWordHit;
        source.Play();
    }
}
