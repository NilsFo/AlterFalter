using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDebug : MonoBehaviour, IPlayerMovementBase
{
    [Header("Player Movement")] public bool movementEnabled = true;
    public PlayerMovementParam movementParams;

    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;
    private Vector2 _velocity;
    private bool _jumpInput;
    private int _jumpsRemaining;
    private float distToGround;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        distToGround = myCollider.bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {
        myRigidbody.angularDrag = movementParams.angularDrag;
        myRigidbody.gravityScale = movementParams.gravityScale;
        myRigidbody.mass = movementParams.mass;
        myRigidbody.drag = movementParams.linearDrag;

        bool jumpingAllowed = movementParams.canJump;
        Vector2 moveInput = new Vector2(0, 0);

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpingAllowed && _jumpsRemaining > 0)
            {
                print("Jumping");
                _jumpInput = true;
            }
            else
            {
                Debug.Log("Jump disabled.");
            }
        }

        if (_jumpInput)
        {
            moveInput.y = movementParams.jumpingForce;
            _jumpsRemaining = _jumpsRemaining - 1;
        }

        // Horizontal Movement
        float horizontalAxis = Input.GetAxis("Horizontal");
        horizontalAxis = horizontalAxis * movementParams.movementForce * Time.deltaTime;
        moveInput.x = horizontalAxis;
        _velocity = moveInput;
    }

    private void FixedUpdate()
    {
        myRigidbody.AddForce(_velocity);
        _jumpInput = false;
    }

    public void OnCollisionWithGround()
    {
        _jumpsRemaining = 1;
    }
}