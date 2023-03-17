using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float knockBackStrength;
    public int damageStrength = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth health = ExtractPlayerHealthComponent(other.gameObject);
        if (health != null)
        {
            DealDamageToPlayer(health);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        PlayerHealth health = ExtractPlayerHealthComponent(col.gameObject);
        if (health != null)
        {
            DealDamageToPlayer(health);
        }
    }

    private PlayerHealth ExtractPlayerHealthComponent(GameObject obj)
    {
        PlayerHealth playerHealth = obj.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            return playerHealth;
        }

        var segmentHealthHealth = obj.GetComponent<WormSegmentHealth>();
        if (segmentHealthHealth != null)
        {
            playerHealth = segmentHealthHealth.playerHealth;
            if (segmentHealthHealth.damageAble)
            {
                return playerHealth;
            }
        }

        return null;
    }

    private void OnCollisionStay(Collision collisionInfo)
    {
        PlayerHealth health = ExtractPlayerHealthComponent(collisionInfo.gameObject);
        if (health != null)
        {
            health.KnockBackPlayer(gameObject, knockBackStrength);
        }
    }

    private void DealDamageToPlayer(PlayerHealth playerHealth)
    {
        playerHealth.TakeDamage(damageStrength);
        playerHealth.KnockBackPlayer(gameObject, knockBackStrength);
    }
}