using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

    public GameObject Player => _player;
    public FlowerCollectible Flower => _blume;

    private CinemachineVirtualCamera _camera;
    public CinemachineVirtualCamera Camera => _camera;

    [Header("Player state")] private PlayerState _lastKnownPlayerState;
    private EvolveState _lastKnownEvolveState;
    public PlayerState playerState;
    public EvolveState evolveState;
    private HealthBar _healthBar;
    private GameObject _player;
    private FlowerCollectible _blume;

    [Header("Caterpillar Food")] public int foodCurrent;
    public int foodTarget;

    [Header("Pupa Evolve")] public int pupaEvolveCurrent = 0;
    public int pupaEvolveTarget = 20;
    public float pupaEvolveDecay = 0.35f;
    private float _pupaEvolveDecayTimer = 0;

    [Header("Butterfly Objective")] public float butterflyFlowerMaxDistance;

    [Header("Callbacks")] public UnityEvent evolveStateChange;
    public UnityEvent gameStateChange;

    public int Food
    {
        get => foodCurrent;
        set => SetFood(value);
    }

    private void Awake()
    {
        _lastKnownPlayerState = PlayerState.Unknown;
        _lastKnownEvolveState = EvolveState.Unknown;
        _healthBar = FindObjectOfType<HealthBar>();
        _blume = FindObjectOfType<FlowerCollectible>();
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        pupaEvolveCurrent = 0;
        _pupaEvolveDecayTimer = 0;
        if (evolveStateChange != null)
        {
            evolveStateChange = new UnityEvent();
        }

        if (gameStateChange != null)
        {
            gameStateChange = new UnityEvent();
        }

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

        //#################
        // Pupa decay
        _pupaEvolveDecayTimer += Time.deltaTime;
        if (_pupaEvolveDecayTimer > pupaEvolveDecay)
        {
            _pupaEvolveDecayTimer = 0;
            pupaEvolveCurrent -= 1;
        }

        if (Input.GetKeyDown(KeyCode.E) && evolveState == EvolveState.Pupa)
        {
            _pupaEvolveDecayTimer = 0;
            pupaEvolveCurrent++;
        }

        pupaEvolveCurrent = Mathf.Clamp(pupaEvolveCurrent, 0, pupaEvolveTarget);

        // Back to Main Menu
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("Menu");
        }

        // Restart Level
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var s = SceneManager.GetActiveScene();
            SceneManager.LoadScene(s.name);
        }
    }

    private void OnPlayStateChange()
    {
        Debug.Log("New Player state: " + playerState);
        gameStateChange.Invoke();
    }

    private void OnEvolveStateChange()
    {
        Debug.Log("New Evolve state: " + evolveState);
        ResetFood();
        evolveStateChange.Invoke();
        _healthBar.OnEvolve();
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

    public void Win()
    {
        playerState = PlayerState.Win;
    }

    public void ResetFood()
    {
        Food = 0;
    }

    private void FixedUpdate()
    {
    }

    public void RegisterPlayer(GameObject newPlayer)
    {
        _player = newPlayer;

        if (_player == null)
        {
            _camera.Follow = null;
        }
        else
        {
            _camera.Follow = _player.transform;
        }
    }

    public float PlayerDistanceToFlower()
    {
        if (Player == null || Flower == null)
        {
            return butterflyFlowerMaxDistance;
        }

        return Vector2.Distance(Player.transform.position, Flower.transform.position);
    }
}