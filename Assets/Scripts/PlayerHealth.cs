using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int healthCurrent;
    public int healthMax = 10;
    public bool invincible;
    public bool knockBackAble;

    private GameState _gameState;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    void Start()
    {
        FullyHeal();
    }

    private void Update()
    {
        if (invincible)
        {
            FullyHeal();
        }

        if (IsDead())
        {
            _gameState.playerState = GameState.PlayerState.Lost;
        }
    }

    private void LateUpdate()
    {
        if (IsDead())
        {
            _gameState.playerState = GameState.PlayerState.Lost;
        }
    }

    public void FullyHeal()
    {
        healthCurrent = healthMax;
    }

    public bool IsDead()
    {
        if (invincible)
        {
            FullyHeal();
            return false;
        }

        return healthCurrent <= 0;
    }

    public float GetHealthPercentage()
    {
        float c = healthCurrent;
        float m = healthMax;
        return c / m;
    }

    public void TakeDamage(int amount = 1)
    {
        if (invincible)
        {
            FullyHeal();
            return;
        }

        healthCurrent -= amount;
        healthCurrent = Math.Max(healthCurrent, 0);

        if (IsDead())
        {
            _gameState.playerState = GameState.PlayerState.Lost;
        }
    }

    public void KnockBackPlayer(GameObject source, float knockBackStrength)
    {
        if (!knockBackAble)
        {
            return;
        }
        
        // TODO implement
        print("TODO: Implement Knockback!");
    }
}