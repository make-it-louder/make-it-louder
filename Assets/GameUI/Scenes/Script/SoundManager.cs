using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class SoundManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider micSlider;
    public AudioSource[] audioSources;
    public TMP_Dropdown micSelector;
    public TMP_Text volumeText;
    public TMP_Text micVolumeText;
    public float microphoneVolume = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        var defaultVolume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = defaultVolume;
        volumeText.text = Mathf.RoundToInt(defaultVolume * 100).ToString();

        var defaultMicVolume = PlayerPrefs.GetFloat("MicVolume", 0.5f);
        micSlider.value = defaultMicVolume;
        micVolumeText.text = Mathf.RoundToInt(defaultMicVolume * 100).ToString();

        if(audioSources.Length != 0) {
            Debug.Log("갈리가없짙");
            foreach (AudioSource source in audioSources)
                {
                    source.volume = defaultVolume;
                }
        }
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

    }

    // Update is called once per frame
    void Update()
    {

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
   
        int intValue = Mathf.RoundToInt(value * 100);
        micVolumeText.text = intValue.ToString();
    }

    // 여기에 플레이어들 볼륨함수? 


}
