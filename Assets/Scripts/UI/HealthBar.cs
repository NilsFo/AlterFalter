using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameState _gameState;
    private PlayerHealth _health;
    public Slider slider;
    public GameObject infinityTF;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    private void OnEnable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        OnEvolve();
        infinityTF.SetActive(false);
    }

    private void Update()
    {
        if (_health != null)
        {
            slider.value = _health.GetHealthPercentage();
            infinityTF.SetActive(_health.invincible);
        }
    }

    public void OnEvolve()
    {
        var player = _gameState.Player;
        _health = null;

        if (_gameState.evolveState == GameState.EvolveState.Unknown)
        {
            return;
        }

        if (player != null)
        {
            PlayerHealth health = player.GetComponentInChildren<PlayerHealth>();
            if (health != null)
            {
                print("Player evolved. New health detected!");
                _health = health;
            }
        }

        if (_health == null)
        {
            Debug.LogError("Player evolved. HEALTH NOT FOUND!!");
        }
    }

    private void OnDestroy()
    {
    }
}