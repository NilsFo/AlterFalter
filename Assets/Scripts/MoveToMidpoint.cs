using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToMidpoint : MonoBehaviour
{
    public Transform objectToMove;
    public Transform object1;
    public Transform object2;

    private Vector3 object1Position;
    private Vector3 object2Position;

    void Update()
    {
        object1Position = object1.position;
        object2Position = object2.position;
        Vector3 midpoint = Vector3.Lerp(object1Position, object2Position, 0.5f);
        objectToMove.position = midpoint;
    }
}