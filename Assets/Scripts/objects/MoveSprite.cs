using UnityEngine;

public class MoveSprite : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Vector2 startPoint;
    public Vector2 endPoint;
    private Vector2 currentTarget;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPoint, 0.1f);
        Gizmos.DrawSphere(endPoint, 0.1f);
        Gizmos.DrawLine(startPoint, endPoint);
    }

    void Start()
    {
        transform.position = startPoint;
        currentTarget = endPoint;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);

        if ((Vector2)transform.position == endPoint)
        {
            currentTarget = startPoint;
        }
        else if ((Vector2)transform.position == startPoint)
        {
            currentTarget = endPoint;
        }
    }
}
