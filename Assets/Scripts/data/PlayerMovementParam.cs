using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerMovementParam", order = 1)]
public class PlayerMovementParam : ScriptableObject
{
    [Header("Config")] public bool canJump = false;

    [Header("Rigidbody Fields")] public float mass = 1.0f;
    public float angularDrag = 0.05f;
    public float linearDrag = 3f;
    public float gravityScale = 1.0f;

    [Header("Gameplay Parameters")] public float movementForce = 100.0f;
    public float jumpingForce = 400.0f;
}