using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Caterpillar_Movement : MonoBehaviour
{
    public float speed = 5f;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode toggleKey = KeyCode.E;
    public KeyCode resetKey = KeyCode.R;
    public Transform worm_end; // assign the object that defines the perimeter in the Inspector
    public Transform worm_start; // assign the second object in the Inspector
    public float perimeterRadius = 3f;
    private Vector3 savedPosition;
    private Vector3 startingPosition;
    private bool followMouse = false;
    private bool canMovePerimeterObject = true;
    private float moveDelay = 0f; // Add a new variable for the delay
    public Tilemap tilemap;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private void Start()
    {
        startingPosition = worm_start.position;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            followMouse = !followMouse;
        }

        if (followMouse)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 newPosition = new Vector2(mousePosition.x, mousePosition.y);
            KeepWithinPerimeter(newPosition);
        }
        else
        {
            float horizontalInput = 0f;

            if (Input.GetKey(leftKey))
            {
                horizontalInput = -1f;
            }
            else if (Input.GetKey(rightKey))
            {
                horizontalInput = 1f;
            }

            if (horizontalInput != 0f && moveDelay <= 0) // Add a condition to check the delay
            {
                Vector2 targetPos = new Vector2(0f, horizontalInput * speed * Time.deltaTime);
                Vector3 worldTargetPos =  transform.TransformPoint(targetPos);
                rb.MovePosition(worldTargetPos);
                //worm_start.Translate(new Vector2(0f, horizontalInput * speed * Time.deltaTime));

                float screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
                Vector3 pos = worm_start.position;
                pos.x = Mathf.Clamp(pos.x, -screenHalfWidth, screenHalfWidth);
                pos.y = worm_start.position.y;
                worm_start.position = pos;
            }

            if (Input.GetKeyDown(resetKey))
            {
                worm_start.position = startingPosition;
            }

            if (worm_end != null)
            {
                Vector3 offset = worm_start.position - worm_end.position;
                float distance = offset.magnitude;
                if (distance > perimeterRadius)
                {
                    Vector3 direction = offset.normalized;
                    worm_start.position = worm_end.position + direction * perimeterRadius;
                }
            }

            // Move the worm_end to the position of the worm_start with a cooldown of 1 second
            if (Input.GetMouseButtonDown(1) && canMovePerimeterObject)
            {
                if (worm_start != null)
                {
                    StartCoroutine(MovePerimeterObject(worm_start.position));
                }
            }

            moveDelay -= Time.deltaTime;
            // Cast a ray from the center of the object's collider downwards
            Vector2 raycastOrigin = transform.position;
            raycastOrigin.y -= GetComponent<Collider2D>().bounds.extents.y;
            Debug.DrawRay(raycastOrigin,Vector2.down);
            RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.1f);

            if (hit.collider != null && hit.collider.gameObject.GetComponent<Tilemap>() == tilemap)
            {
                // Debug.Log("Collision detected with tilemap!");
                // Do something here, e.g. destroy the object or trigger some other action
            }

            SnapSecondObject();
        }

        IEnumerator MovePerimeterObject(Vector3 targetPosition)
        {
            canMovePerimeterObject = false;
            float elapsedTime = 0f;
            float duration = 0.05f;
            moveDelay = duration; // Set the delay to the Lerp duration
            Vector3 startPosition = worm_end.position;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                worm_end.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
                yield return null;
            }

            canMovePerimeterObject = true;
        }

        //OnCollisionEnter2D();

        void SnapSecondObject()
        {
            if (worm_start != null)
            {
                Vector3 offset = worm_end.position - worm_start.position;
                float distance = offset.magnitude;
                if (distance >= perimeterRadius)
                {
                    Vector3 direction = offset.normalized;
                    worm_end.position = worm_start.position + direction * perimeterRadius;
                    StartCoroutine(MovePerimeterObject(worm_start.position));
                }
            }
        }
//


        void KeepWithinPerimeter(Vector2 newPosition)
        {
            if (worm_end != null)
            {
                Vector3 offset = newPosition - (Vector2)worm_end.position;
                float distance = offset.magnitude;
                if (distance > perimeterRadius)
                {
                    Vector3 direction = offset.normalized;
                    newPosition = worm_end.position + direction * perimeterRadius;
                }
            }

            worm_start.position = newPosition;
        }
    }

}