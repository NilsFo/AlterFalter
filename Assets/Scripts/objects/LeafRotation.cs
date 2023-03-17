using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float rotationAngle = 15.0f;
    private float currentAngle = 0.0f;
    private float initialAngle;
    private bool direction = true;

    void Start()
    {
        initialAngle = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        float angleStep = rotationSpeed * Time.deltaTime;

        if (direction)
        {
            currentAngle += angleStep;
            if (currentAngle > rotationAngle)
            {
                currentAngle = rotationAngle;
                direction = false;
            }
        }
        else
        {
            currentAngle -= angleStep;
            if (currentAngle < -rotationAngle)
            {
                currentAngle = -rotationAngle;
                direction = true;
            }
        }

        transform.rotation = Quaternion.Euler(0, 0, initialAngle + currentAngle);
    }
}
