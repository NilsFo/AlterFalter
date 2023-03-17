using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private GameState _gameState;
    public GameObject plantParticlePoofPrefab;
    
    private MusicManager _musicManager;
    public AudioClip plantCollect;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
        _musicManager = FindObjectOfType<MusicManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerInventory playerInventory = col.GetComponent<PlayerInventory>();

        if (playerInventory != null && _gameState.evolveState == GameState.EvolveState.Caterpillar)
        {
            var poof = Instantiate(plantParticlePoofPrefab);
            poof.transform.position = transform.position;

            playerInventory.PlantsCollected();
            gameObject.SetActive(false);
            _musicManager.CreateAudioClip(plantCollect);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        var gameState = FindObjectOfType<GameState>();
        gameState.foodTarget += 1;
    }

    // Update is called once per frame
    void Update()
    {
    }
}