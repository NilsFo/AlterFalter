using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvolveableButterfly : MonoBehaviour, IEvolveable
{
    private GameState _gameState;
    public UnityEvent onEvolve;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameState.evolveState = GameState.EvolveState.Butterfly;
        _gameState.RegisterPlayer(gameObject);
        if (onEvolve != null)
        {
            onEvolve = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            print("A butterfly can not evolve. You may not like it, but this is what peak evolution looks like!");
        }
    }

    public void Evolve()
    {
        throw new System.NotImplementedException();
    }

    public bool CanEvolve()
    {
        return false;
    }

    public void OnEvolve()
    {
        onEvolve.Invoke();
    }
}