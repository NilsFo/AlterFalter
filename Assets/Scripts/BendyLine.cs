using System;
using UnityEngine;

public class BendyLine : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public float bendDistance;
    public float lineThickness;
    public Color lineColor;
    public Material lineMaterial;

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineThickness;
        lineRenderer.endWidth = lineThickness;
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

        float distance = Vector3.Distance(pos1, pos2);

        if (distance <= bendDistance)
        {
            Vector3[] positions = new Vector3[15];
            positions[0] = pos1;
            for (int i = 1; i < positions.Length - 1; i++)
            {
                float t = (float)i / (positions.Length - 1);
                Vector3 pos = Vector3.Lerp(pos1, pos2, t);
                Vector3 dir = pos - pos1;
                Vector3 dirHalf = dir * 0.5f;
                Vector3 dirMid = pos1 + dirHalf;

                float bendT = distance / bendDistance;
                float angle = Mathf.Lerp(0f, Mathf.PI / 2f, bendT);
                Vector3 norm = new Vector3(dirHalf.y, -dirHalf.x, 0f).normalized;
                norm = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward) * norm;

                Vector3 mid = dirMid + norm * (bendDistance - distance) * 0.5f;
                Vector3 up = new Vector3(0f, 1f, 0f);

                positions[i] = mid + up * (float)i / (positions.Length - 1) * (pos2.y - pos1.y);
            }
            positions[positions.Length - 1] = pos2;

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }
        else
        {
            Vector3[] positions = new Vector3[2];
            positions[0] = pos1;
            positions[1] = pos2;

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }
    }
}