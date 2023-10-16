using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerSliderManager : MonoBehaviour
{
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            slider.value = Mathf.Clamp(slider.value - Time.deltaTime, 0.0f, 1.0f);
        }
        if (Input.GetKey(KeyCode.E))
        {
            slider.value = Mathf.Clamp(slider.value + Time.deltaTime, 0.0f, 1.0f);
        }
    }
}
