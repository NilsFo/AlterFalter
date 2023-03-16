using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBugAI : MonoBehaviour
{
    public enum AIState
    {
        Walking,
        Taunting
    }

    [Header("AI")] public AIState currentAI;
    private AIState _lastKnownAI;

    [Header("Walking")] public bool walkLeft;
    public float walkSpeed;
    public AnimationCurve accelerationCurve;
    private float _walkingTime;
    private float _turnAroundCooldown;

    [Header("Data")] public Animator myAnimator;
    public Rigidbody2D rb;

    private void Awake()
    {
        _lastKnownAI = AIState.Walking;
    }

    // Start is called before the first frame update
    void Start()
    {
        _walkingTime = 0;
        _turnAroundCooldown = 0;
    }

    private void Update()
    {
        _turnAroundCooldown = _turnAroundCooldown - Time.deltaTime;
        var scale = transform.localScale;
        if (walkLeft)
        {
            scale.x = 1;
        }
        else
        {
            scale.x = -1;
        }

        if (currentAI == AIState.Walking)
        {
            _walkingTime += Time.deltaTime;
        }
        else
        {
            _walkingTime = 0;
        }

        transform.localScale = scale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentAI == AIState.Walking)
        {
            // Moving
            Vector3 pos = transform.position;
            float x = accelerationCurve.Evaluate(_walkingTime) * walkSpeed * Time.fixedDeltaTime;
            x = x * -1;

            Vector2 targetPos = new Vector2(x, 0);
            Vector3 worldTargetPos = transform.TransformPoint(targetPos);
            rb.MovePosition(worldTargetPos);
        }
    }

    public void BowlAway()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Caterpillar_Movement caterpillarMovement = col.gameObject.GetComponent<Caterpillar_Movement>();
        if (caterpillarMovement != null)
        {
            TurnAround();
        }
    }

    public void TurnAround()
    {
        if (_turnAroundCooldown <= 0)
        {
            _turnAroundCooldown = 0.2f;
            _walkingTime = 0;
            walkLeft = !walkLeft;
        }
    }
}