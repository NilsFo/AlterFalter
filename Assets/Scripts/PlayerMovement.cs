using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")] public bool movementEnabled = true;
    public PlayerMovementParam movementParams;

    private Rigidbody2D myRigidbody;
    private Collider2D myCollider;

    public float movementForce;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //myRigidbody.angularDrag = movementParams.angularDrag;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Vector2 force = new Vector2(movementForce, 0);
            myRigidbody.AddForce(force);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vector2 force = new Vector2(-movementForce, 0);
            myRigidbody.AddForce(force);
        }
    }

}