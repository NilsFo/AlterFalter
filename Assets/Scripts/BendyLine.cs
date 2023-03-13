using UnityEngine;

public class BendyLine : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public Color lineColor;
    public Material lineMaterial;
    public LineRenderer lineRenderer;
    public int numPoints = 7;
    public float maxDistance = 5f;
    public float bendDistanceFactor = 1f;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
    }

    void Update()
    {
        if (object1 == null || object2 == null)
        {
            return;
        }

        Vector3 pos1 = object1.transform.position;
        Vector3 pos2 = object2.transform.position;

        // Calculate the midpoint between the two endpoints
        Vector3 midPoint = Vector3.Lerp(pos1, pos2, 0.5f);

        // Calculate the distance between the two endpoints
        float distance = Vector3.Distance(pos1, pos2);

        // Calculate the control point for the bezier curve
        Vector3 controlPoint = midPoint + new Vector3(0, (2f - distance) * bendDistanceFactor, 0);

        // Calculate the positions for the line renderer using a bezier curve
        Vector3[] positions = new Vector3[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            float t = (float)i / (numPoints - 1);
            Vector3 p1 = Vector3.Lerp(pos1, controlPoint, t);
            Vector3 p2 = Vector3.Lerp(controlPoint, pos2, t);
            positions[i] = Vector3.Lerp(p1, p2, t);
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }
}
