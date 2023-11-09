using RainbowArt.CleanFlatUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBarManager : MonoBehaviour
{
    ProgressBar progressBar;
    SliderRange sliderRange;
    public MicInputManager micInputManager;
    public float physicalMinVolume = -50.0f;
    public float physicalMaxVolume =  20.0f;
    // Start is called before the first frame update
    void Start()
    {
        progressBar = GetComponentInChildren<ProgressBar>();
        sliderRange = GetComponentInChildren<SliderRange>();
        sliderRange.MinValue = physicalMinVolume;
        sliderRange.MaxValue = physicalMaxVolume;
        progressBar.MaxValue = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.CurrentValue = micInputManager.normalizedDB;
        progressBar.transform.Find("Foreground Area").Find("Foreground").GetComponent<Image>().color = Color.HSVToRGB(progressBar.CurrentValue, 1, 1);
    }
}
