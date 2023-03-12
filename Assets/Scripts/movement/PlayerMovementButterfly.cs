using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementButterfly : MonoBehaviour, IPlayerMovementBase
{

    private GameState _gameState;
    private Vector2 _velocity;
    public bool movementEnabled = true;
    public float movementSpeed = 100;

    public Rigidbody2D myRigidBody;
    
    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    private void Start()
    {
        _velocity = new Vector2();
        _gameState.evolveState = GameState.EvolveState.Butterfly;
        _gameState.player = gameObject;
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2();
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        
        if (!movementEnabled)
        {
            moveInput = Vector2.zero;
        }

        _velocity = moveInput;
        _velocity = _velocity.normalized;
        _velocity *= movementSpeed;
    }

    private void FixedUpdate()
    {
        myRigidBody.AddForce(_velocity * Time.fixedDeltaTime * 100);
    }
}