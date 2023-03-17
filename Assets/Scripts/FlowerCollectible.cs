using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCollectible : MonoBehaviour
{
    private GameState _gameState;
    private MusicManager _musicManager;
    public GameObject flowerPoof;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
        _musicManager = FindObjectOfType<MusicManager>();
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
        PlayerMovementButterfly btf = go.GetComponent<PlayerMovementButterfly>();
        if (btf != null)
        {
            if (_gameState.evolveState == GameState.EvolveState.Butterfly)
            {
                gameObject.SetActive(false);
                _gameState.Win();
                
                var poof = Instantiate(flowerPoof);
                var pos = transform.position;
                pos.z = transform.position.z - 1;
                poof.transform.position = pos;
            }
        }
    }
}