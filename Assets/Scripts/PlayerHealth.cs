using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int healthCurrent;
    public int healthMax = 10;
    public bool invincible;

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
}