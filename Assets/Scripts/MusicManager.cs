using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public GameObject temporalAudioPlayerPrefab;
    public static float userDesiredVolume = 0.5f;
    public readonly float GLOBAL_VOLUME_MULT = 0.5f;
    private bool playingIntro = false;

    private AudioListener _listener;
    public AudioSource songIntro, songLoop, stingerWin, stringerLoose;
    private List<AudioSource> playList;

    private void Awake()
    {
        playList = new List<AudioSource>();
        playList.Add(songIntro);
        playList.Add(songLoop);
        playList.Add(stingerWin);
        playList.Add(stringerLoose);

        _listener = FindObjectOfType<AudioListener>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Play(int i)
    {
        foreach (AudioSource audioSource in playList)
        {
            audioSource.Stop();
        }

        playList[i].Play();
    }

    public void PlaySongIntro()
    {
        playingIntro = true;
        Play(0);
    }

    public void PlaySongLoop()
    {
        playingIntro = false;
        Play(1);
    }

    public void PlayStingerWin()
    {
        playingIntro = false;
        Play(2);
    }

    public void PlayStringerLoose()
    {
        playingIntro = false;
        Play(3);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _listener.transform.position;

        if (playingIntro)
        {
            if (!songIntro.isPlaying)
            {
                playingIntro = false;
                PlaySongLoop();
            }
        }
    }

    public float GetVolume()
    {
        return userDesiredVolume * GLOBAL_VOLUME_MULT;
    }

    public void CreateAudioClip(AudioClip audioClip)
    {
        var adp = Instantiate(temporalAudioPlayerPrefab,transform);
        AudioSource source = adp.GetComponent<AudioSource>();
        source.clip = audioClip;
        source.Play();
    }
}