using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    public static SFX instance;
    AudioSource source;

    public AudioClip TileSuccessSFX;

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
}
