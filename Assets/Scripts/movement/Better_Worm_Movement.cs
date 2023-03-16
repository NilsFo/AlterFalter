using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Better_Worm_Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float wallClimbSpeed = 3f;
    public float rotationSpeed = 10f;
    public LayerMask tilemapLayer;
    public Vector3 boxSize;
    public float maxDistanceFromWormEnd = 3f;
    public KeyCode toggleMouseControlKey = KeyCode.T;
    public float stayFixedDuration = 5f;
    private float unfixedTime;
    private Rigidbody2D wormRigidbody;
    private CapsuleCollider2D wormCollider;
    private Collider2D wormEndCollider;
    public float snapCooldown = 1f;
    public float snapDuration = 0.15f;
    private float lastSnapTime;
    private float horizontal;
    private float vertical;
    private Quaternion targetRotation;
    private bool mouseControl = false;
    public Rigidbody2D wormEndRigidbody;
    private WormEndController wormEndController;
    private bool isSnappingInProgress;
    private float remainingStayFixedDuration;
    public GameObject wormEnd;
    private FixedJoint2D wormEndFixedJoint; 



    void Start()
    {
        wormRigidbody = GetComponent<Rigidbody2D>();
        wormCollider = GetComponent<CapsuleCollider2D>();
        boxSize = new Vector3(wormCollider.size.x * transform.localScale.x, wormCollider.size.y * transform.localScale.y, 0);
        targetRotation = transform.rotation;
        lastSnapTime = -snapCooldown; // Initialize lastSnapTime to allow snapping at the start
        wormEndRigidbody = wormEnd.GetComponent<Rigidbody2D>();
        // Get the reference to the worm_end's WormEndController script
        wormEndController = wormEnd.GetComponent<WormEndController>();
        wormEndCollider = wormEnd.GetComponent<Collider2D>();
        wormEndFixedJoint = wormEnd.AddComponent<FixedJoint2D>();
        wormEndFixedJoint.enabled = false;
    }

    void Update()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

    if (Input.GetKeyDown(toggleMouseControlKey))
    {
        mouseControl = !mouseControl;
    }

        if (!isSnappingInProgress)
        {
            if (mouseControl)
            {
                FollowMouse();
            }
            else
            {
               MoveLeftRight();
                MoveUpDown();
                MoveOnCeiling();
            }

            RotateWorm();
        }

        // Check if the distance between the worm and worm_end is equal to or greater than maxDistanceFromWormEnd or the worm is touching a wall
        //float distance = Vector3.Distance(transform.position, wormEnd.transform.position);
        //if (!mouseControl && distance >= maxDistanceFromWormEnd && Time.time - lastSnapTime >= snapCooldown && IsTouchingTilemap() && Time.time >= unfixedTime)
        //{
        //    MoveWormEndToWorm();
       // }


        if (Input.GetMouseButtonDown(1))
        {
            MoveWormEndToWorm();
        }
        // Drag the worm towards the worm_end if it's outside the perimeter, and if snapping is not in progress
        if (!isSnappingInProgress)
        {
            float distance = Vector3.Distance(transform.position, wormEnd.transform.position);
            if (distance > maxDistanceFromWormEnd)
            {
                Vector3 direction = (wormEnd.transform.position - transform.position).normalized;
                Vector3 newPosition = transform.position + direction * (distance - maxDistanceFromWormEnd);
                wormRigidbody.MovePosition(newPosition);
            }
        }
} 

    void MoveLeftRight()
    {
        Vector3 newPosition = transform.position + new Vector3(horizontal * moveSpeed * Time.deltaTime, 0, 0);
        if (IsWithinPerimeter(newPosition))
        {
            wormRigidbody.velocity = new Vector2(horizontal * moveSpeed, wormRigidbody.velocity.y);
        }
    }

    void MoveUpDown()
    {
        if (Mathf.Abs(vertical) > 0.01f)
        {
            RaycastHit2D hitLeft = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.left, 0, tilemapLayer);
            RaycastHit2D hitRight = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.right, 0, tilemapLayer);
            RaycastHit2D hitDown = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.down, 0, tilemapLayer);

            if (hitLeft.collider != null || hitRight.collider != null)
            {
                Vector3 newPosition = transform.position + new Vector3(0, vertical * wallClimbSpeed * Time.deltaTime, 0);
                if (IsWithinPerimeter(newPosition))
                {
                    wormRigidbody.velocity = new Vector2(wormRigidbody.velocity.x, vertical * wallClimbSpeed);
                }
            }
            else if (hitDown.collider != null && (hitLeft.collider != null || hitRight.collider != null))
            {
                // If the worm is in a corner with a tilemap below and either left or right of it, allow it to move up
                Vector3 newPosition = transform.position + new Vector3(0, Mathf.Max(vertical, 0) * moveSpeed * Time.deltaTime, 0);
                if (IsWithinPerimeter(newPosition))
                {
                    wormRigidbody.velocity = new Vector2(wormRigidbody.velocity.x, Mathf.Max(vertical, 0) * moveSpeed);
                }
            }
        }
    }



    void MoveOnCeiling()
    {
        RaycastHit2D hitUp = Physics2D.BoxCast(transform.position, boxSize, 0, Vector2.up, 0, tilemapLayer);
        if (hitUp.collider != null && Mathf.Abs(horizontal) > 0.01f)
        {
            Vector3 newPosition = transform.position + new Vector3(horizontal * moveSpeed * Time.deltaTime, 0, 0);
            if (IsWithinPerimeter(newPosition))
            {
                wormRigidbody.velocity = new Vector2(horizontal * moveSpeed, wormRigidbody.velocity.y);
            }
        }
    }

void FollowMouse()
{
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = 0;

    Vector3 direction = (mousePosition - wormEnd.transform.position).normalized;
    float distance = Vector3.Distance(mousePosition, wormEnd.transform.position);

    if (distance > maxDistanceFromWormEnd)
    {
        mousePosition = wormEnd.transform.position + direction * maxDistanceFromWormEnd;
    }

    Vector3 movementDirection = (mousePosition - transform.position);
    float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
    targetRotation = Quaternion.Euler(0, 0, targetAngle);

    // Scale the movement speed based on the distance from the worm to the mouse position
    float speedScale = Mathf.Clamp01(distance / maxDistanceFromWormEnd);

    // Use Rigidbody2D to move the worm while considering physics and collisions
    Vector3 newPosition = transform.position + new Vector3(movementDirection.x * moveSpeed * speedScale * Time.deltaTime, movementDirection.y * moveSpeed * speedScale * Time.deltaTime, 0);
    if (IsWithinPerimeter(newPosition))
    {
        wormRigidbody.velocity = new Vector2(movementDirection.x * moveSpeed * speedScale, movementDirection.y * moveSpeed * speedScale);
    }
    else
    {
        wormRigidbody.velocity = Vector2.zero;
    }
}

    void RotateWorm()
    {
        Vector2 movementDirection = new Vector2(horizontal, vertical);
        if (movementDirection.magnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
            targetRotation = Quaternion.Euler(0, 0, targetAngle);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

void MoveWormEndToWorm()
{
    if (Time.time - lastSnapTime >= snapCooldown)
    {
        Vector3 startPosition = wormEnd.transform.position;
        Vector3 targetPosition = transform.position;

        // Check if there's a tilemap collider at the target position
        float extraDistance = 0.1f; // Increase this value if needed
        CapsuleCollider2D capsuleCollider = wormEndCollider as CapsuleCollider2D;
        Vector2 size = capsuleCollider.size + new Vector2(extraDistance, extraDistance);

        bool canSnap = CheckCollisionAtPosition(targetPosition, size, capsuleCollider.direction, tilemapLayer);
        if (canSnap && IsTouchingTilemap())
        {
            StartCoroutine(MoveWormEndToWormSmoothly(startPosition, targetPosition, snapDuration));
            lastSnapTime = Time.time;
        }
    }
}



bool IsWithinPerimeter(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(targetPosition, wormEnd.transform.position);
        return distance <= maxDistanceFromWormEnd;
    }
    // Check if the worm is colliding with the current tilemap
    bool IsTouchingTilemap()
    {
        float extraDistance = 0.1f;
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(wormCollider.bounds.center, wormCollider.size, wormCollider.direction, 0f, Vector2.zero, extraDistance, tilemapLayer);
        return raycastHit.collider != null;
    }
    bool IsWormEndTouchingTilemap()
{
    Collider2D[] results = new Collider2D[1];
    int overlapCount = Physics2D.OverlapCollider(wormEndCollider, new ContactFilter2D().NoFilter(), results);
    for (int i = 0; i < overlapCount; i++)
    {
        if (results[i].gameObject.layer == LayerMask.NameToLayer("Tilemap"))
        {
            return true;
        }
    }
    return false;
}


// Move the worm_end to the worm smoothly
IEnumerator MoveWormEndToWormSmoothly(Vector3 startPosition, Vector3 targetPosition, float duration)
{
    isSnappingInProgress = true; // Set the snapping state to true

    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;
        wormEnd.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
        yield return null;
    }

    wormEnd.transform.position = targetPosition;

    // Only enable the worm_end FixedJoint if it's colliding with something
    if (wormEndController.isColliding)
    {
        wormEndFixedJoint.enabled = true;
        StartCoroutine(DisableWormEndFixedJoint());
    }

    isSnappingInProgress = false; // Set the snapping state to false
}


    // Coroutine to re-enable worm_end physics after stayFixedDuration
    IEnumerator EnableWormEndPhysics()
    {
        remainingStayFixedDuration = stayFixedDuration; // Set the remaining duration to the initial value
        while (remainingStayFixedDuration > 0f)
        {
            remainingStayFixedDuration -= Time.deltaTime; // Decrease the remaining duration
            yield return null;
        }

        remainingStayFixedDuration = 0f;
        wormEndRigidbody.isKinematic = false;
        wormEndRigidbody.simulated = true;
    }
        // Add a new method to get the remaining stay fixed duration
    public float GetRemainingStayFixedDuration()
    {
        return remainingStayFixedDuration;
    }
IEnumerator DisableWormEndFixedJoint()
{
    remainingStayFixedDuration = stayFixedDuration; // Set the remaining duration to the initial value
    while (remainingStayFixedDuration > 0f)
    {
        remainingStayFixedDuration -= Time.deltaTime; // Decrease the remaining duration
        yield return null;
    }

    remainingStayFixedDuration = 0f;
    wormEndFixedJoint.enabled = false;
}
bool CheckCollisionAtPosition(Vector3 position, Vector2 size, CapsuleDirection2D direction, LayerMask layer)
{
    Collider2D hitCollider = Physics2D.OverlapCapsule(position, size, direction, 0f, layer);
    return hitCollider != null;
}
}
