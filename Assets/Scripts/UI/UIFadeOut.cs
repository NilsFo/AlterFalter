using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFadeOut : MonoBehaviour
{
    public CanvasRenderer myRenderer;

    public float initialAlpha;
    public float delay;
    private float delay_current;
    public float alphaChangeRate;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer.SetAlpha(initialAlpha);
        delay_current = 0;
    }

    // Update is called once per frame
    void Update()
    {
        delay_current += Time.deltaTime;
        if (delay_current > delay)
        {
            float alpha = myRenderer.GetAlpha();
            alpha += alphaChangeRate;
            alpha = Math.Clamp(alpha, 0, 1);

            myRenderer.SetAlpha(alpha);
        }
    }
}