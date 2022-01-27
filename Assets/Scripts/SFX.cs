using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX instance;
    AudioSource source;

    public AudioClip TileSuccessSFX;
    public AudioClip HitFailSFX;
    public AudioClip badWordPlaced;
    public AudioClip okWordPlaced;
    public AudioClip goodWordPlaced;

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
        source.PlayOneShot(source.clip);
    }

    public void PlayOkWordPlacedSFX()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = okWordPlaced;
        source.PlayOneShot(source.clip);
    }

    public void PlayBadWordPlacedSFX()
    {
        source.pitch = Random.Range(0.75f, 1.25f);
        source.clip = badWordPlaced;
        source.PlayOneShot(source.clip);
    }
}
