using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
            newObject.transform.SetParent(transform); // Set the parent of the new object to the PlayerCaterpillar
            spriteObjects[i - 1] = newObject; // Decrement i to match the new array index
            SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
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

    void Update()
    {
        for (int i = 1; i < lineRenderer.positionCount - 1; i++) // Start from index 1, end at second-to-last index
        {
            Vector3 position = lineRenderer.GetPosition(i);
            spriteObjects[i - 1].transform.position = position; // Decrement i to match the new array index
        }
    }
}
