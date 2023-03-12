using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementPuppa : MonoBehaviour, IPlayerMovementBase
{
    private GameState _gameState;
    private Vector2 _velocity;

    [Header("Movement")] public bool movementEnabled = true;
    public float movementSpeed = 100;
    public float accelerationTimer = 0f;
    public float accelerationMult = 1.0f;
    public AnimationCurve accelerationCurve;

    [Header("Animation")] public GameObject mySpriteHolder;

    private Rigidbody2D _myRigidBody;
    private Vector2 _lastFramePosition;
    private Vector2 _ridgitbodyVelocity;

    private void Awake()
    {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _gameState = FindObjectOfType<GameState>();
        _lastFramePosition = _myRigidBody.position;
        _ridgitbodyVelocity = _myRigidBody.position;
    }

    private void Start()
    {
        _velocity = new Vector2();
        _gameState.evolveState = GameState.EvolveState.Pupa;
        _gameState.player = gameObject;
    }

    private void Update()
    {
        // Updating Movement / Translation
        Vector2 moveInput = new Vector2();
        var playerInput = Input.GetAxis("Horizontal");
        bool leftMode = playerInput > 0;

        if (IsPlayerInputPressed())
        {
            accelerationTimer += Time.deltaTime * accelerationMult;
            float curvedAcceleration = accelerationCurve.Evaluate(accelerationTimer);

            if (leftMode)
            {
                curvedAcceleration = curvedAcceleration * -1;
            }

            moveInput.x = curvedAcceleration;
        }
        else
        {
            accelerationTimer = 0;
            moveInput.x = 0;
        }

        if (!movementEnabled)
        {
            moveInput = Vector2.zero;
            moveInput.x = 0;
        }

        _velocity = moveInput;
        _velocity = _velocity.normalized;
        _velocity *= movementSpeed;

        // Updating Visuals
        print(_ridgitbodyVelocity);
    }

    private void FixedUpdate()
    {
        _myRigidBody.AddForce(_velocity * Time.fixedDeltaTime * 100);

        _ridgitbodyVelocity = (_myRigidBody.position - _lastFramePosition) / Time.fixedDeltaTime;
        _lastFramePosition = _myRigidBody.position;
    }

    public bool IsPlayerInputPressed()
    {
        return Input.GetAxisRaw("Horizontal") != 0;
    }
}