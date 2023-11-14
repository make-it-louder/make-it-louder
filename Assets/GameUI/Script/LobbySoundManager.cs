using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LobbySoundManager : MonoBehaviour
{

    private float volume;
    private Slider slider;
    
    public AudioMixerGroup mixer;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        volume = PlayerPrefs.GetFloat("BackGround", 0.5f);

        slider.value = volume;
        mixer.audioMixer.SetFloat("BgVolume", calcLogDB(volume));
    }

    public void OnValueChanged(float value)
    {
        volume = value;
        PlayerPrefs.SetFloat("BackGround", volume);
        PlayerPrefs.Save();

        mixer.audioMixer.SetFloat("BgVolume", calcLogDB(volume));
    }
    private float calcLogDB(float value)
    {
        return Mathf.Log10(value) * 20;
    }
}
