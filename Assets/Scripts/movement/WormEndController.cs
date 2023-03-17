using UnityEngine;

public class WormEndController : MonoBehaviour
{
    public bool isColliding;
    public LayerMask tilemapLayer;
    public float checkRadius = 0.1f;

    private CapsuleCollider2D capsuleCollider;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        CheckCollision();
    }

    void CheckCollision()
    {
        Vector2 capsuleColliderSize = new Vector2(capsuleCollider.size.x * transform.localScale.x, capsuleCollider.size.y * transform.localScale.y);
        Collider2D[] results = new Collider2D[1];
        int hitCount = Physics2D.OverlapCapsuleNonAlloc(transform.position, capsuleColliderSize, capsuleCollider.direction, 0, results, tilemapLayer);

        if (hitCount > 0)
        {
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }
    }
}
