using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSegment : MonoBehaviour
{
    public Rigidbody2D rb;
    public TimedLife myLife;

    private void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Release(float velocity)
    {
        rb.constraints = RigidbodyConstraints2D.None;

        var pos = transform.position;
        pos.z = -9;
        transform.position = pos;

        Vector2 vel = new Vector2();
        vel.x = velocity * 10f;
        vel.y = Mathf.Abs(velocity) * 5f;

        rb.AddForce(vel);
        myLife.timerActive = true;
    }
}