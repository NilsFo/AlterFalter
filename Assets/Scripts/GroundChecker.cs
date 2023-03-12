using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundChecker : MonoBehaviour
{

    [FormerlySerializedAs("myPlayerMovement")] public PlayerMovementDebug myPlayerMovementDebug;

    private void OnTriggerEnter2D(Collider2D col)
    {
        myPlayerMovementDebug.OnCollisionWithGround();
    }
}
