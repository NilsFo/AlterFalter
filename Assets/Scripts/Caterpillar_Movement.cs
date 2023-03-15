using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caterpillar_Movement : MonoBehaviour
{
    public float speed = 5f;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode toggleKey = KeyCode.E;
    public KeyCode resetKey = KeyCode.R;
    public Transform perimeterObject; // assign the object that defines the perimeter in the Inspector
    public float perimeterRadius = 3f;
    public Transform secondObject; // assign the second object in the Inspector
    private Vector3 savedPosition; // to save the position of perimeterObject when right mouse button is pressed
    private Vector3 startingPosition;
    private bool followMouse = false;

    private bool canMovePerimeterObject = true; // add this variable to track whether the action can be performed

    private void Start()
    {
        startingPosition = transform.position;
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
            transform.position = new Vector2(mousePosition.x, mousePosition.y);
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

            if (horizontalInput != 0f)
            {
                transform.Translate(new Vector2(horizontalInput * speed * Time.deltaTime, 0f));

                // Ensure the object stays within the screen boundaries
                float screenHalfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
                Vector3 pos = transform.position;
                pos.x = Mathf.Clamp(pos.x, -screenHalfWidth, screenHalfWidth);
                pos.y = transform.position.y;
                transform.position = pos;
            }
        }

        if (Input.GetKeyDown(resetKey))
        {
            transform.position = startingPosition;
        }

        // Ensure the first object stays within the perimeter of the perimeterObject
        if (perimeterObject != null)
        {
            Vector3 offset = transform.position - perimeterObject.position;
            float distance = offset.magnitude;
            if (distance > perimeterRadius)
            {
                Vector3 direction = offset.normalized;
                transform.position = perimeterObject.position + direction * perimeterRadius;
            }
        }

        // Move the perimeter object to the position of the second object with a cooldown of 1 second
        if (Input.GetMouseButtonDown(1) && canMovePerimeterObject)
        {
            if (secondObject != null)
            {
                StartCoroutine(MovePerimeterObject(secondObject.position));
            }
        }
    }

    private IEnumerator MovePerimeterObject(Vector3 targetPosition)
    {
        canMovePerimeterObject = false; // set the variable to false to prevent further actions

        // move the perimeter object to the target position over a period of 1 second
        float elapsedTime = 0f;
        Vector3 startPosition = perimeterObject.position;
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            perimeterObject.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            yield return null;
        }

        canMovePerimeterObject = true; // set the variable to true to allow the action to be performed again
    }
    
}