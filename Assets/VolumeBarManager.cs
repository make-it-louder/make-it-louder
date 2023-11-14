using RainbowArt.CleanFlatUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBarManager : MonoBehaviour
{
    Slider progressBar;
    SliderRange sliderRange;
    public MicInputManager micInputManager;
    public float physicalMinVolume = -50.0f;
    public float physicalMaxVolume =  20.0f;
    // Start is called before the first frame update
    void Start()
    {
        progressBar = GetComponentInChildren<Slider>();
        sliderRange = GetComponentInChildren<SliderRange>();
        sliderRange.MinValue = physicalMinVolume;
        sliderRange.MaxValue = physicalMaxVolume;
        progressBar.minValue = physicalMinVolume;
        progressBar.maxValue = physicalMaxVolume;
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.value = micInputManager.DB;
        micInputManager.minDB = sliderRange.Value1;
        micInputManager.maxDB = sliderRange.Value2;
    }
}
