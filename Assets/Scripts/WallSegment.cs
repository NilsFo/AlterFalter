using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSegment : MonoBehaviour
{
    public Rigidbody2D rb;

    private void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Release(float velocity)
    {
        rb.constraints = RigidbodyConstraints2D.None;
    }
}