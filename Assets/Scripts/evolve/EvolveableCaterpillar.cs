using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolveableCaterpillar : MonoBehaviour, IEvolveable
{
    private GameState _gameState;
    public GameObject nextEvolvePrefab;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameState.evolveState = GameState.EvolveState.Caterpillar;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            print("evolve!");
        }
        
    }

    public bool CanEvolve()
    {
        return _gameState.Food == _gameState.foodTarget;
    }

    public void Evolve()
    {
    }
}