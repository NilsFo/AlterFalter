using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLife : MonoBehaviour
{
    public float aliveTime = 4.0f;
    private float _timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        _timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= aliveTime)
        {
            DestroySelf();
        }
    }

    [ContextMenu("Destroy Self")]
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}