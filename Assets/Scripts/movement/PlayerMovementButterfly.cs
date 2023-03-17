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
    public GameObject pupaPoof;
    public SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        _gameState = FindObjectOfType<GameState>();
    }

    private void Start()
    {
        _velocity = new Vector2();

        var poof = Instantiate(pupaPoof);
        var pos = transform.position;
        pos.z = transform.position.z - 1;
        poof.transform.position = pos;
    }

    private void Update()
    {
        if (_gameState.playerState == GameState.PlayerState.Lost)
        {
            movementEnabled = false;
        }

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

        mySpriteRenderer.color = _gameState.currentDmgTint;
    }

    private void FixedUpdate()
    {
        myRigidBody.AddForce(_velocity * Time.fixedDeltaTime * 100);
    }
}