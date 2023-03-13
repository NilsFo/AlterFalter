using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioListener _listener;

    // Start is called before the first frame update
    void Start()
    {
        _listener = FindObjectOfType<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = _listener.transform.position;
    }
}