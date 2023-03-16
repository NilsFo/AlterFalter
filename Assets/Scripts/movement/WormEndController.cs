using UnityEngine;

public class WormEndController : MonoBehaviour
{
    public bool isColliding;

    void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
    }
}
