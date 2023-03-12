using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSway : MonoBehaviour
{
    public GameObject affectingGameObject;

    public float swaySpeed;
    public float swayMagnitude;
    private float _swayProgress;

    // Start is called before the first frame update
    void Start()
    {
        _swayProgress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _swayProgress += Time.deltaTime * swaySpeed;

        float sway = MathF.Sin(_swayProgress) * swayMagnitude;
        Vector3 rot = affectingGameObject.transform.rotation.eulerAngles;
        rot.z = sway;
        affectingGameObject.transform.rotation = Quaternion.Euler(rot);
    }
}