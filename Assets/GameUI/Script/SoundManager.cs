using Photon.Voice.PUN;
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


        micInput = FindObjectOfType<MicInput>();
        InitializeMicSelector();

        micSelector.onValueChanged.AddListener(OnMicSelectorValueChanged);
    }
    // Start is called before the first frame update

    private MicInput micInput;


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

    private float  calcAudioMixerVolume(float value)
    {
        return Mathf.Log10(value) * 20;
    }
    private void OnEnable()
    {
        InitializeMicSelector();
    }
}
