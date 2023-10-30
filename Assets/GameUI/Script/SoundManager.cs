using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class SoundManager : MonoBehaviour
{
    //Slider
    public Slider volumeSlider;
    public Slider micSlider;
    //Text(Number)
    public TMP_Text volumeText;
    public TMP_Text micVolumeText;

    public AudioSource[] audioSources;
    public TMP_Dropdown micSelector;

    private string selectedMicName;
    private AudioSource micAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        var defaultVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = defaultVolume;
        volumeText.text = Mathf.RoundToInt(defaultVolume * 100).ToString();

        var defaultMicVolume = PlayerPrefs.GetFloat("MicVolume", 0.8f);
        micSlider.value = defaultMicVolume;
        micVolumeText.text = Mathf.RoundToInt(defaultMicVolume * 100).ToString();

        if(audioSources.Length != 0) {
            Debug.Log("갈리가없짙");
            foreach (AudioSource source in audioSources)
                {
                    source.volume = defaultVolume;
                }
        }

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
        int audioFrequency = 44100;

        // 마이크를 시작하고 입력을 재생
        micAudioSource = gameObject.AddComponent<AudioSource>();
        micAudioSource.clip = Microphone.Start(selectedMicName, true, 10, audioFrequency);
        micAudioSource.volume = micSlider.value;
        micAudioSource.loop = true;
        while (Microphone.GetPosition(selectedMicName) <= 0) { }
        micAudioSource.Play();
    }



    public void OnMicSelectorValueChanged(int micIndex)
    {
        selectedMicName = micSelector.options[micIndex].text;
        // 여기에서 선택한 마이크 이름을 저장
        PlayerPrefs.SetString("SelectedMic", selectedMicName);
        PlayerPrefs.Save();

        // 마이크를 시작하고 입력을 재생
        int audioFrequency = 44100;
        micAudioSource.clip = Microphone.Start(selectedMicName, true, 10, audioFrequency);
        micAudioSource.volume = micSlider.value;
        while (Microphone.GetPosition(selectedMicName) <= 0) { }
        micAudioSource.Play();
    }

    public void ChangeVolume(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();

        foreach (AudioSource source in audioSources)
        {
            source.volume = value;
        }

        // volumeText 텍스트 업데이트
        int intValue = Mathf.RoundToInt(value * 100);
        volumeText.text = intValue.ToString();
        }
    public void ChangeMicVolume(float value)
    {
        PlayerPrefs.SetFloat("MicVolume", value);
        PlayerPrefs.Save();
        if (micAudioSource != null)
        {
            micAudioSource.volume = value;
        }
        int intValue = Mathf.RoundToInt(value * 100);
        micVolumeText.text = intValue.ToString();
    }





    // 여기에 플레이어들 볼륨함수? 


}
