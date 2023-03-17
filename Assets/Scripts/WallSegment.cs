using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class WallSegment : MonoBehaviour
{
    public Rigidbody2D rb;
    public TimedLife myLife;
    public Collider2D myCollider;

    private float _vel = 0f;

    private void Start()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void Release(float velocity)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.WakeUp();
        myCollider.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Clutter");

        var pos = transform.position;
        pos.z = -9;
        transform.position = pos;
        _vel = velocity;
        Invoke(nameof(ApplyVelocity), 0.01f);

        myLife.timerActive = true;
    }

    private void ApplyVelocity()
    {
        print("pushing! "+_vel);
        rb.WakeUp();
        rb.constraints = RigidbodyConstraints2D.None;
        rb.WakeUp();
        Vector2 vel = new Vector2();
        vel.x = _vel * 20.1337f;
        vel.y = Mathf.Abs(_vel) * 6.9f;

        rb.AddForce(vel);
    }
}