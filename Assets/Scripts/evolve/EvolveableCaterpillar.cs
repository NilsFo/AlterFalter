using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EvolveableCaterpillar : MonoBehaviour, IEvolveable
{
    private GameState _gameState;
    public GameObject nextEvolvePrefab;
    public UnityEvent onEvolve;

    public GameObject cameraTarget;
    public GameObject evolveCreationLocation;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameState.RegisterPlayer(gameObject);
        _gameState.evolveState = GameState.EvolveState.Caterpillar;
        _gameState.Camera.Follow = cameraTarget.transform;

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
            if (CanEvolve())
            {
                Evolve();
            }
            else
            {
                print("Not enough food.");
            }
        }
    }

    public bool CanEvolve()
    {
        return _gameState.Food >= _gameState.foodTarget;
    }

    public void Evolve()
    {
        print("Caterpillar: Evolving!");
        Vector3 pos = evolveCreationLocation.transform.position;
        Destroy(gameObject);
        Instantiate(nextEvolvePrefab, pos, Quaternion.identity);
    }

    public void OnEvolve()
    {
        onEvolve.Invoke();
    }
}