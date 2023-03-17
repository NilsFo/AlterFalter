using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceAutoVolume : MonoBehaviour
{
    private MusicManager _musicManager;
    public AudioSource myAudioSource;

    private void Awake()
    {
        _musicManager = FindObjectOfType<MusicManager>();
    }

    // Update is called once per frame
    void Update()
    {
        myAudioSource.volume = _musicManager.GetVolume();
    }
}