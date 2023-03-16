using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public float attackVelocityThreshold = 0;

    [Header("Animation")] public GameObject mySpriteHolder;
    public float spinningUpRotationSpeed = 5;
    public float accelerationSpriteRotationSpeed = 1.0f;
    public float velocitySpriteRotationMult = 1.0f;
    public float uprightAlignmentRotationSpeed = 1.0f;

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
    }

    private void Update()
    {
        if (_gameState.playerState == GameState.PlayerState.Lost)
        {
            movementEnabled = false;
        }

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
        float horizontal_velocity = -_ridgitbodyVelocity.x * velocitySpriteRotationMult;
        if (IsPlayerInputPressed())
        {
            float temporalBaseRotationSpeed = spinningUpRotationSpeed * Time.deltaTime;
            horizontal_velocity = Mathf.Max(temporalBaseRotationSpeed, -_ridgitbodyVelocity.x);
            if (leftMode && -_ridgitbodyVelocity.x <= 0)
            {
                horizontal_velocity = Mathf.Min(temporalBaseRotationSpeed * -1, -_ridgitbodyVelocity.x);
            }

            horizontal_velocity = horizontal_velocity * accelerationSpriteRotationSpeed;
        }

        if (!IsPlayerInputPressed() && _ridgitbodyVelocity.x == 0)
        {
            // Get the current up direction vector
            Vector3 currentUpDirection = transform.up;

            // Calculate the angle between the current up direction and the world up direction
            float angleToUp = Vector3.Angle(currentUpDirection, Vector3.up);

            // If the angle is greater than a small threshold value, rotate the transform towards the world up direction
            if (angleToUp > 0.1f)
            {
                float step = uprightAlignmentRotationSpeed * Time.deltaTime;
                Vector3 newUpDirection = Vector3.RotateTowards(currentUpDirection, Vector3.up, step, 0.0f);
                mySpriteHolder.transform.rotation =
                    Quaternion.LookRotation(mySpriteHolder.transform.forward, newUpDirection);
            }
            // If the angle is small enough, set the transform's up direction to the world up direction
            else
            {
                mySpriteHolder.transform.up = Vector3.up;
            }
        }
        else
        {
            mySpriteHolder.transform.Rotate(0, 0, horizontal_velocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        EnemyBugAI enemyBugAI = col.gameObject.GetComponent<EnemyBugAI>();
        float velocity = _ridgitbodyVelocity.x;
        if (enemyBugAI != null)
        {
            if (velocity >= attackVelocityThreshold)
            {
                enemyBugAI.BowlAway();
            }
            else
            {
                enemyBugAI.TurnAround();
            }
        }
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

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Vector3 position = transform.position;
        // Handles.DrawWireDisc(wireOrigin, Vector3.forward, 1);
        Handles.Label(position, "Vel: " + _ridgitbodyVelocity.x);
#endif
    }
}