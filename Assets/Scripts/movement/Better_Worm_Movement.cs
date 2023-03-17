// Import the necessary namespaces

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// The main class for the worm movement
public class Better_Worm_Movement : MonoBehaviour
{
    // Public variables that can be set in the Unity editor
    public float moveSpeed = 5f; // The worm's movement speed
    public float wallClimbSpeed = 3f; // The worm's climbing speed
    public float rotationSpeed = 10f; // The speed of the worm's rotation
    public LayerMask tilemapLayer; // The layer on which the tilemap is located
    public Vector3 boxSize; // The size of the worm's box collider
    public float maxDistanceFromWormEnd = 3f; // The maximum distance the worm end can be from the worm
    public float stayFixedDuration = 5f; // Duration for which the worm end stays fixed
    private float unfixedTime; // Time when the worm end becomes unfixed
    private Rigidbody2D wormRigidbody; // Reference to the worm's Rigidbody2D component
    private CapsuleCollider2D wormCollider; // Reference to the worm's CapsuleCollider2D component
    private Collider2D wormEndCollider; // Reference to the worm_end's Collider2D component
    public float snapCooldown = 1f; // Cooldown time between consecutive snaps
    public float snapDuration = 0.15f; // Duration of the snapping process
    private float lastSnapTime; // Time when the last snap occurred
    private float horizontal; // Horizontal input axis value
    private float vertical; // Vertical input axis value
    private Quaternion targetRotation; // The target rotation of the worm
    private bool mouseControl = true; // Boolean to check if the mouse control is enabled
    public Rigidbody2D wormEndRigidbody; // Reference to the worm_end's Rigidbody2D component
    private WormEndController wormEndController; // Reference to the worm_end's WormEndController script
    private bool isSnappingInProgress; // Boolean to check if snapping is in progress
    private float remainingStayFixedDuration; // Remaining duration for which the worm end stays fixed
    public GameObject wormEnd; // The worm_end game object
    private FixedJoint2D wormEndFixedJoint; // Reference to the worm_end's FixedJoint2D component

    // Start method is called when the script is first enabled
    // TODO: Disable Controls when game is lost!
    // TODO implement knockback

    void Start()
    {
        wormRigidbody = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the worm
        wormCollider = GetComponent<CapsuleCollider2D>(); // Get the CapsuleCollider2D component of the worm
        boxSize = new Vector3(wormCollider.size.x * transform.localScale.x,
            wormCollider.size.y * transform.localScale.y,
            0); // Calculate the box size based on the worm's collider size and scale
        targetRotation = transform.rotation; // Set the initial target rotation to the worm's current rotation
        lastSnapTime = -snapCooldown; // Initialize lastSnapTime to allow snapping at the start
        wormEndRigidbody = wormEnd.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the worm_end
        wormEndController =
            wormEnd.GetComponent<WormEndController>(); // Get the reference to the worm_end's WormEndController script
        wormEndCollider = wormEnd.GetComponent<Collider2D>(); // Get the Collider2D component of the worm_end
        wormEndFixedJoint = wormEnd.AddComponent<FixedJoint2D>(); //Add a FixedJoint2D component to the worm_end
        wormEndFixedJoint.enabled = false; // Disable the FixedJoint2D component initially
        _gameState = FindObjectOfType<GameState>();

    }


// The Update method is called once per frame
    void Update()
    {
        // Get the horizontal and vertical input axis values
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // If snapping is not in progress, perform movement actions
        if (!isSnappingInProgress)
        {

            FollowMouse();
        


            // Rotate the worm
            RotateWorm();
        }

        // The commented out code checks if the distance between the worm and worm_end is equal to or greater than maxDistanceFromWormEnd
        // or the worm is touching a wall, and then moves the worm end to the worm

        // If the right mouse button is clicked, check if snapping is allowed and move the worm_end to the worm

    if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (Time.time - lastSnapTime >= snapCooldown && (IsTouchingTilemap() ||
                                                             (remainingStayFixedDuration > 0 &&
                                                              wormEndController.isColliding)))
            {
                MoveWormEndToWorm();
            }
        }

        // Drag the worm towards the worm_end if it's outside the perimeter, and if snapping is not in progress
        if (!isSnappingInProgress)
        {
            float distance = Vector3.Distance(transform.position, wormEnd.transform.position);
            if (distance > maxDistanceFromWormEnd)
            {
                // Calculate the direction and new position for the worm to be dragged towards the worm_end
                Vector3 direction = (wormEnd.transform.position - transform.position).normalized;
                Vector3 newPosition = transform.position + direction * (distance - maxDistanceFromWormEnd);
                wormRigidbody.MovePosition(newPosition);
            }
        }
    }

    void FollowMouse()
    {
        // Get the mouse position in the world and normalize the z-axis
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Calculate the direction and distance from the worm_end to the mouse position
        Vector3 direction = (mousePosition - wormEnd.transform.position).normalized;
        float distance = Vector3.Distance(mousePosition, wormEnd.transform.position);

        // If the distance is greater than the maximum allowed distance, limit it
        if (distance > maxDistanceFromWormEnd)
        {
            mousePosition = wormEnd.transform.position + direction * maxDistanceFromWormEnd;
        }

        // Calculate the movement direction and target rotation angle for the worm
        Vector3 movementDirection = (mousePosition - transform.position);
        float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0, 0, targetAngle);

        // Scale the movement speed based on the distance from the worm to the mouse position
        float speedScale = Mathf.Clamp01(distance / maxDistanceFromWormEnd);

        // Use Rigidbody2D to move the worm while considering physics and collisions
        Vector3 newPosition = transform.position +
                              new Vector3(movementDirection.x * moveSpeed * speedScale * Time.deltaTime,
                                  movementDirection.y * moveSpeed * speedScale * Time.deltaTime, 0);
        if (IsWithinPerimeter(newPosition))
        {
            wormRigidbody.velocity = new Vector2(movementDirection.x * moveSpeed * speedScale,
                movementDirection.y * moveSpeed * speedScale);
        }
        else
        {
            wormRigidbody.velocity = Vector2.zero;
        }
    }

// Rotate the worm based on the input
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

// Draw a wire cube in the Scene view for debugging purposes
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }

// Move the worm_end to the worm's position
    void MoveWormEndToWorm()
    {
        Vector3 targetPosition = transform.position;
        if (Time.time - lastSnapTime >= snapCooldown && (IsTouchingTilemap() ||
                                                         IsPositionCollidingWithTilemap(targetPosition,
                                                             wormEndCollider.bounds.size)))
        {
            Vector3 startPosition = wormEnd.transform.position;
            StartCoroutine(MoveWormEndToWormSmoothly(startPosition, targetPosition, snapDuration));
            lastSnapTime = Time.time;
        }
    }


    bool IsWithinPerimeter(Vector3 targetPosition)
    {
        // Calculate distance between the target position and worm_end
        float distance = Vector3.Distance(targetPosition, wormEnd.transform.position);
        // Check if the distance is within the maximum allowed distance from the worm_end
        return distance <= maxDistanceFromWormEnd;
    }

    bool IsTouchingTilemap()
    {
        // Define extra distance for the CapsuleCast
        float extraDistance = 0.1f;
        // Perform a CapsuleCast with the worm's collider
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(wormCollider.bounds.center, wormCollider.size,
            wormCollider.direction, 0f, Vector2.zero, extraDistance, tilemapLayer);
        // Check if the CapsuleCast collided with a tilemap
        return raycastHit.collider != null;
    }

    bool IsWormEndTouchingTilemap()
    {
        // Create an array to store colliders
        Collider2D[] results = new Collider2D[1];
        // Perform an OverlapCollider check with the worm_end's collider
        int overlapCount = Physics2D.OverlapCollider(wormEndCollider, new ContactFilter2D().NoFilter(), results);
        // Check if any of the overlapping colliders belong to the "Default" layer
        for (int i = 0; i < overlapCount; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                Debug.Log("touching");
                return true;
            }
        }

        return false;
    }

// Move the worm_end to the worm smoothly
    IEnumerator MoveWormEndToWormSmoothly(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        // Set the snapping state to true
        isSnappingInProgress = true;

        // Initialize elapsed time
        float elapsedTime = 0f;

        // Smoothly move the worm_end from startPosition to targetPosition over the specified duration
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            wormEnd.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            yield return null;
        }

        // Set the final position of the worm_end
        wormEnd.transform.position = targetPosition;

        // Only enable the worm_end FixedJoint if it's colliding with something
        if (wormEndController.isColliding)
        {
            Debug.Log("worm end colliding");
            wormEndFixedJoint.enabled = true;
            StartCoroutine(DisableWormEndFixedJoint());
        }

        // Set the snapping state to false
        isSnappingInProgress = false;
    }

// Coroutine to re-enable worm_end physics after stayFixedDuration
    IEnumerator EnableWormEndPhysics()
    {
        // Set the remaining duration to the initial value
        remainingStayFixedDuration = stayFixedDuration;
        // Decrease the remaining duration
        while (remainingStayFixedDuration > 0f)
        {
            remainingStayFixedDuration -= Time.deltaTime;
            yield return null;
        }

        // Reset the remaining duration and enable worm_end physics
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

            // Add a small delay before checking if worm_end is not colliding with the tilemap anymore
            //yield return new WaitForSeconds(1f);

            // Check if worm_end is not colliding with the tilemap anymore
            if (!wormEndController.isColliding)
            {
                // If not colliding, immediately disable the fixed joint and exit the coroutine
                remainingStayFixedDuration = 0f;
                wormEndFixedJoint.enabled = false;
                yield break;
            }

            yield return null;
        }

        remainingStayFixedDuration = 0f;
        wormEndFixedJoint.enabled = false;
    }

    bool IsPositionCollidingWithTilemap(Vector3 position, Vector2 size)
    {
        Collider2D[] results = new Collider2D[1];
        int overlapCount = Physics2D.OverlapBox(position, size, 0, new ContactFilter2D().NoFilter(), results);
        for (int i = 0; i < overlapCount; i++)
        {
            if (results[i].gameObject.layer == LayerMask.NameToLayer("Default"))
            {
                return true;
            }
        }

        return false;
    }
}