using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameState : MonoBehaviour
{
    public enum PlayerState
    {
        Unknown,
        Playing,
        Paused,
        Win,
        Lost
    }

    public enum EvolveState
    {
        Unknown,
        Caterpillar,
        Pupa,
        Butterfly
    }

    private CinemachineVirtualCamera _camera;

    [Header("Player state")] private PlayerState _lastKnownPlayerState;
    private EvolveState _lastKnownEvolveState;
    public PlayerState playerState;
    public EvolveState evolveState;
    public GameObject player;

    [Header("Caterpillar Food")] public int foodCurrent;

    public int Food
    {
        get => foodCurrent;
        set => SetFood(value);
    }

    public int foodTarget;


    private void Awake()
    {
        _lastKnownPlayerState = PlayerState.Unknown;
        _lastKnownEvolveState = EvolveState.Unknown;
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetFood();
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastKnownEvolveState != evolveState)
        {
            OnEvolveStateChange();
            _lastKnownEvolveState = evolveState;
        }

        if (_lastKnownPlayerState != playerState)
        {
            OnPlayStateChange();
            _lastKnownPlayerState = playerState;
        }

        if (player == null)
        {
            _camera.Follow = null;
        }
        else
        {
            _camera.Follow = player.transform;
        }
    }

    private void OnPlayStateChange()
    {
        Debug.Log("New Player state: " + playerState);
    }

    private void OnEvolveStateChange()
    {
        Debug.Log("New Evolve state: " + evolveState);
    }

    [ContextMenu("Add 1 food")]
    public void AddFood()
    {
        Food = Food + 1;
    }

    private int SetFood(int newFood)
    {
        foodCurrent = newFood;
        if (foodCurrent >= foodTarget)
        {
            foodCurrent = foodTarget;
        }

        return Food;
    }

    public void ResetFood()
    {
        Food = 0;
    }

    private void FixedUpdate()
    {
    }
}