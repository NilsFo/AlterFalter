using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Caterpillar_Segment_Creator : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Sprite sprite;
    public GameState gameState; // Add a reference to the GameState script
    public float flashInterval = 0.1f;
    public float flashDurationRedTint = 0.5f;


    private GameObject[] spriteObjects;
    private bool isFlashing = false;

    void Start()
    {
        spriteObjects = new GameObject[lineRenderer.positionCount - 2]; // Ignore the first and last positions

        for (int i = 1; i < lineRenderer.positionCount - 1; i++) // Start from index 1, end at second-to-last index
        {
            Vector3 position = lineRenderer.GetPosition(i);
            GameObject newObject = new GameObject("Worm_Seg_" + i.ToString());
            newObject.transform.position = position;
            newObject.transform.SetParent(transform); // Set the parent of the new object to the PlayerCaterpillar
            spriteObjects[i - 1] = newObject; // Decrement i to match the new array index
            SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            gameState = FindObjectOfType<GameState>(); 
        }
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        for (int i = 1; i < lineRenderer.positionCount - 1; i++) // Start from index 1, end at second-to-last index
        {
            Vector3 position = lineRenderer.GetPosition(i);
            Vector3 wireOrigin = new Vector3(position.x, position.y, position.z - 1);
            // Handles.DrawWireDisc(wireOrigin, Vector3.forward, 1);
            Handles.Label(wireOrigin, "S: " + i);
        }
#endif
    }
void FixedUpdate()
{
    for (int i = 1; i < lineRenderer.positionCount - 1; i++) // Start from index 1, end at second-to-last index
    {
        Vector3 position = lineRenderer.GetPosition(i);
        spriteObjects[i - 1].transform.position = position; // Decrement i to match the new array index
        SpriteRenderer spriteRenderer = spriteObjects[i - 1].GetComponent<SpriteRenderer>();

        // Apply the currentDmgTint color directly from the GameState
        spriteRenderer.color = gameState.currentDmgTint;
    }

    CheckForRedTint();
}


void CheckForRedTint()
{
    if (gameState.currentDmgTint == Color.red && !isFlashing)
    {
        isFlashing = true;
        StartCoroutine(FlashRed(gameState.currentDmgTint));
    }
}


private IEnumerator FlashRed(Color originalColor)
{
    while (gameState.currentDmgTint == Color.red)
    {
        // Change the color to red for all segments
        foreach (GameObject segment in spriteObjects)
        {
            segment.GetComponent<SpriteRenderer>().color = Color.red;
        }

        // Wait for the fixed interval
        yield return new WaitForFixedUpdate();

        // Revert the color to the original color for all segments
        foreach (GameObject segment in spriteObjects)
        {
            segment.GetComponent<SpriteRenderer>().color = originalColor;
        }

        // Wait for the fixed interval
        yield return new WaitForFixedUpdate();
    }

    isFlashing = false;
}







}
