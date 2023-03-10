using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerMovementParam", order = 1)]
public class PlayerMovementParam : ScriptableObject
{
    public bool canJump=false;
    public float velocity = 0;
    public float angularDrag;
}