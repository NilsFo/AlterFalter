using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuVolumeSlider : MonoBehaviour
{

    public Slider mySlider;
    
    // Start is called before the first frame update
    void Start()
    {
        mySlider.value = MusicManager.userDesiredVolume;
    }

    // Update is called once per frame
    void Update()
    {
        float sliderValue = mySlider.value;
        MusicManager.userDesiredVolume = sliderValue;
    }
}
