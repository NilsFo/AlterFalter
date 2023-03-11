using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{

    public PlayerMovement myPlayerMovement;

    private void OnTriggerEnter2D(Collider2D col)
    {
        myPlayerMovement.OnCollisionWithGround();
    }
}
