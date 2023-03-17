using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBugFeeler : MonoBehaviour
{
    public EnemyBugAI bugAI;

    private void OnTriggerEnter2D(Collider2D col)
    {
        TilemapCollider2D tmc = col.gameObject.GetComponent<TilemapCollider2D>();
        EnemyBugAI otherBugAI = col.gameObject.GetComponent<EnemyBugAI>();
        Caterpillar_Movement caterpillarMovement = col.gameObject.GetComponent<Caterpillar_Movement>();
        BreakableWall wall = col.gameObject.GetComponent<BreakableWall>();

        if (tmc != null || otherBugAI != null || wall != null)
        {
            // Enemy bug collided with tile map
            if (bugAI.currentAI == EnemyBugAI.AIState.Walking)
            {
                bugAI.TurnAround();
            }
        }
    }
}