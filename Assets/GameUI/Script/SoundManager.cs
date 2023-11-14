using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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

        [SerializeField]
        public string playerPrefName;
    }
    [Header("MicSelector")]
    public TMP_Dropdown micSelector;
    private string selectedMicName;

    [Header("SynchronizedVoiceSettings")]
    [SerializeField]
    private SoundEventManager soundEventManager;

    [Header("MixerGroupSettings")]
    //Slider
    [SerializeField]
    private MixerGroupSettings background;
    
    [SerializeField]
    private MixerGroupSettings effect;
    
    [SerializeField]
    private MixerGroupSettings other;


    // Start is called before the first frame update
    void Awake()
    {
        var defaultBgVolume = PlayerPrefs.GetFloat(background.playerPrefName, 0.5f);

        if (background.slider != null)
        {
            background.slider.value = defaultBgVolume;
        }
        background.textArea.text = Mathf.RoundToInt(defaultBgVolume * 100).ToString();

        var defaultEffectVolume = PlayerPrefs.GetFloat(effect.playerPrefName, 0.5f);
        if (effect.slider != null)
        {
            effect.slider.value = defaultEffectVolume;
        }
        effect.textArea.text = Mathf.RoundToInt(defaultEffectVolume * 100).ToString();
        var defaultOtherMicVolume = PlayerPrefs.GetFloat(other.playerPrefName, 0.5f);
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
        Debug.Log("마이크바뀜");
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

    public void ChangeBgVolume(float value)
    {
        PlayerPrefs.SetFloat(background.playerPrefName, value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        background.textArea.text = intValue.ToString();

        background.audioMixerGroup.audioMixer.SetFloat("BgVolume", calcLogDB(value));
    }

    public void ChangeEffectVolume(float value)
    {
        PlayerPrefs.SetFloat(effect.playerPrefName, value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        effect.textArea.text = intValue.ToString();

        effect.audioMixerGroup.audioMixer.SetFloat("FXVolume", calcLogDB(value));
    }
    public void ChangeOtherMicVolume(float value)
    {
        PlayerPrefs.SetFloat(other.playerPrefName, value);
        PlayerPrefs.Save();
        int intValue = Mathf.RoundToInt(value * 100);
        other.textArea.text = intValue.ToString();

        other.audioMixerGroup.audioMixer.SetFloat("OtherMicVolume", calcLogDB(value));
    }

    private float  calcLogDB(float value)
    {
        return Mathf.Log10(value) * 20;
    }



    // 여기에 플레이어들 볼륨함수? 


}
