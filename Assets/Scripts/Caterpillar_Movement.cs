using UnityEngine;

public class Caterpillar_Movement : MonoBehaviour
{
    public float speed = 5f;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode toggleKey = KeyCode.E;
    public KeyCode resetKey = KeyCode.R;

    private Vector3 startingPosition;
    private bool followMouse = false;

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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
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
                transform.Translate(new Vector2(0f,horizontalInput * speed * Time.deltaTime));

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
    }
}