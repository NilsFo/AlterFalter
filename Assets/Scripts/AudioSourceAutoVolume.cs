using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceAutoVolume : MonoBehaviour
{
    private MusicManager _musicManager;
    public AudioSource myAudioSource;
    public float audioSourceMult = 1.0f;

    private void Awake()
    {
        _musicManager = FindObjectOfType<MusicManager>();
    }

    // Update is called once per frame
    void Update()
    {
        myAudioSource.volume = _musicManager.GetVolume()*audioSourceMult;
    }
}