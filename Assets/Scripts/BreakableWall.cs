using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public List<WallSegment> mySegments;
    public BoxCollider2D collisionCol;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovementPuppa p = collision.gameObject.GetComponent<PlayerMovementPuppa>();
        if (p != null)
        {
            PlayerCol(p.GetVelocity());
        }
    }

    public void PlayerCol(float velocity)
    {
        foreach (var wallSegment in mySegments)
        {
            wallSegment.transform.parent = null;
            wallSegment.Release(velocity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        collisionCol.enabled = false;
        PlayerMovementPuppa p = col.gameObject.GetComponent<PlayerMovementPuppa>();
        if (p != null)
        {
            PlayerCol(p.GetVelocity());
        }
    }
}