using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class UIGameStateDisplayer : MonoBehaviour
{
    private GameState _gameState;
    public GameObject affectingObject;
    public GameState.PlayerState playerStateVisible = GameState.PlayerState.Unknown;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }


    // Update is called once per frame
    void Update()
    {
        affectingObject.SetActive(_gameState.playerState == playerStateVisible);
    }
}