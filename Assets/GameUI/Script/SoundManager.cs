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

        float minVolume = -80.0f;
        float maxVolume = 0.0f;
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

        InitializeMicSelector();

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
        selectedMicName = PlayerPrefs.GetString("SelectedMic", ""); // 기본값은 빈 문자열

        if (!string.IsNullOrEmpty(selectedMicName))
        {
            int micIndex = Array.IndexOf(myMIC, selectedMicName);
            if (micIndex != -1)
            {
                micSelector.value = micIndex;
            }
        }

        // 마이크 선택 후 초기화
        micSelector.onValueChanged.AddListener(OnMicSelectorValueChanged);
        //int audioFrequency = 44100;

        //// 마이크를 시작하고 입력을 재생1
        //micAudioSource = gameObject.AddComponent<AudioSource>();
        //micAudioSource.clip = Microphone.Start(selectedMicName, true, 10, audioFrequency);
        //micAudioSource.volume = micSlider.value;
        //micAudioSource.loop = true;
        //while (Microphone.GetPosition(selectedMicName) <= 0) { }
        //micAudioSource.Play();
    }



    public void OnMicSelectorValueChanged(int micIndex)
    {
        selectedMicName = micSelector.options[micIndex].text;
        // 여기에서 선택한 마이크 이름을 저장
        PlayerPrefs.SetString("SelectedMic", selectedMicName);
        PlayerPrefs.Save();

        // 마이크를 시작하고 입력을 재생
        //int audioFrequency = 44100;
        //micAudioSource.clip = Microphone.Start(selectedMicName, true, 10, audioFrequency);
        //micAudioSource.volume = micSlider.value;
        //while (Microphone.GetPosition(selectedMicName) <= 0) { }
        //micAudioSource.Play();
    }

    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        volume.textArea.text = intValue.ToString();

        volume.audioMixerGroup.audioMixer.SetFloat("BGFXVolume", calcLogDB(value));
    }

    public void ChangeMicVolume(float value)
    {
        PlayerPrefs.SetFloat("MicVolume", value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        mic.textArea.text = intValue.ToString();

        volume.audioMixerGroup.audioMixer.SetFloat("MyMicVolume", calcLogDB(volume.slider.minValue));
        soundEventManager.SetMicVolume(value);
    }
    public void ChangeOtherMicVolume(float value)
    {
        PlayerPrefs.SetFloat("OtherMicVolume", value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        other.textArea.text = intValue.ToString();

        volume.audioMixerGroup.audioMixer.SetFloat("OtherMicVolume", calcLogDB(value));
    }

    private float  calcLogDB(float value)
    {
        return Mathf.Log10(value) * 20;
    }



    // 여기에 플레이어들 볼륨함수? 


}
