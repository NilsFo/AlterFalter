using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    public List<WallSegment> mySegments;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovementPuppa p = collision.gameObject.GetComponent<PlayerMovementPuppa>();
        if (p != null)
        {
            Destroy(gameObject);

            foreach (var wallSegment in mySegments)
            {
                wallSegment.Release(p.GetVelocity());
            }
        }
    }
}