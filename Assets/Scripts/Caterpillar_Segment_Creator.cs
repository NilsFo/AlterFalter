using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caterpillar_Segment_Creator : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Sprite sprite;
    private GameObject[] spriteObjects;

    void Start()
    {
        spriteObjects = new GameObject[lineRenderer.positionCount - 2]; // Ignore the first and last positions

        for (int i = 1; i < lineRenderer.positionCount - 1; i++) // Start from index 1, end at second-to-last index
        {
            Vector3 position = lineRenderer.GetPosition(i);
            GameObject newObject = new GameObject("Worm_Seg_" + i.ToString());
            newObject.transform.position = position;
            spriteObjects[i - 1] = newObject; // Decrement i to match the new array index
            SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
        }
    }

    void Update()
    {
        for (int i = 1; i < lineRenderer.positionCount - 1; i++) // Start from index 1, end at second-to-last index
        {
            Vector3 position = lineRenderer.GetPosition(i);
            spriteObjects[i - 1].transform.position = position; // Decrement i to match the new array index
        }
    }
}
