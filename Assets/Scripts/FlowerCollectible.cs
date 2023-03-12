using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCollectible : MonoBehaviour
{
    private GameState _gameState;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Collision?");

        GameObject go = col.gameObject;
        go.GetComponent<PlayerMovementButterfly>();
        if (go != null)
        {
            gameObject.SetActive(false);
            _gameState.playerState = GameState.PlayerState.Win;
        }
    }
}