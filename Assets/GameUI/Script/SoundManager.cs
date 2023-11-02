using Photon.Voice.PUN;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class SoundManager : MonoBehaviour
{
    [Serializable]
    class MixerGroupSettings
    {
        [SerializeField]
        public Slider slider;
        
        [SerializeField]
        public TMP_Text textArea;
        
        [SerializeField]
        public AudioMixerGroup audioMixerGroup;
    }
    [Header("MixerGroupSettings")]
    //Slider
    [SerializeField]
    private MixerGroupSettings volume;
    
    [SerializeField]
    private MixerGroupSettings mic;
    
    [SerializeField]
    private MixerGroupSettings other;

    [Header("SynchronizedVoiceSettings")]
    [SerializeField]
    private SoundEventManager soundEventManager;

    [Header("MicSelector")]
    public TMP_Dropdown micSelector;
    private string selectedMicName;
    // Start is called before the first frame update

    private MicInput micInput;

    void Start()
    {
        var defaultVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        if (volume.slider != null)
        {
            volume.slider.value = defaultVolume;
        }
        volume.textArea.text = Mathf.RoundToInt(defaultVolume * 100).ToString();

        var defaultMicVolume = PlayerPrefs.GetFloat("MicVolume", 0.8f);
        if (mic.slider != null)
        {
            mic.slider.value = defaultMicVolume;
        }
        mic.textArea.text = Mathf.RoundToInt(defaultMicVolume * 100).ToString();
        soundEventManager.SetMicVolume(defaultMicVolume);

        var defaultOtherMicVolume = PlayerPrefs.GetFloat("OtherMicVolume", 1.0f);
        if (other.slider != null)
        {
            other.slider.value = defaultOtherMicVolume;
        }
        other.textArea.text = Mathf.RoundToInt(defaultOtherMicVolume * 100).ToString();


        micInput = FindObjectOfType<MicInput>();
        InitializeMicSelector();

        micSelector.onValueChanged.AddListener(OnMicSelectorValueChanged);
    }


    void InitializeMicSelector()
    {
        string[] myMIC = Microphone.devices;
        Debug.Log(myMIC.Length);
        micSelector.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach (string deviceName in myMIC)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(deviceName);
            options.Add(option);
        }
        micSelector.AddOptions(options);

        // 저장된 마이크 선택 (이전에 선택한 마이크를 복원)
        selectedMicName = PlayerPrefs.GetString("SelectedMic", null);
        if (Array.IndexOf(myMIC, selectedMicName) == -1 && myMIC.Length > 0)
        {
            selectedMicName = myMIC[0];
        }
        if (!string.IsNullOrEmpty(selectedMicName))
        {
            int micIndex = Array.IndexOf(myMIC, selectedMicName);
            if (micIndex != -1)
            {
                micSelector.value = micIndex;
                if (micInput == null)
                {
                    micInput = FindObjectOfType<MicInput>();
                    if (micInput == null)
                    {
                        return;
                    }
                }
                micInput.SetMicName(selectedMicName);
            }
        }
    }



    public void OnMicSelectorValueChanged(int micIndex)
    {
        Debug.Log("마이크바뀜");
        selectedMicName = micSelector.options[micIndex].text;
        // 여기에서 선택한 마이크 이름을 저장
        PlayerPrefs.SetString("SelectedMic", selectedMicName);
        PlayerPrefs.Save();

        if (micInput == null)
        {
            micInput = FindObjectOfType<MicInput>();
            if (micInput == null)
            {
                Debug.Log("Cannot find micInput");
                return;
            }
        }
        micInput.SetMicName(selectedMicName);
    }

    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        volume.textArea.text = intValue.ToString();

        volume.audioMixerGroup.audioMixer.SetFloat("BGFXVolume", calcAudioMixerVolume(value));
    }

    public void ChangeMicVolume(float value)
    {
        PlayerPrefs.SetFloat("MicVolume", value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        mic.textArea.text = intValue.ToString();

        volume.audioMixerGroup.audioMixer.SetFloat("MyMicVolume", calcAudioMixerVolume(volume.slider.minValue));
        soundEventManager.SetMicVolume(value);
    }
    public void ChangeOtherMicVolume(float value)
    {
        PlayerPrefs.SetFloat("OtherMicVolume", value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        other.textArea.text = intValue.ToString();

        volume.audioMixerGroup.audioMixer.SetFloat("OtherMicVolume", calcAudioMixerVolume(value));
    }

    private float  calcAudioMixerVolume(float value)
    {
        return Mathf.Log10(value) * 20;
    }
    private void OnEnable()
    {
        InitializeMicSelector();
    }
}
