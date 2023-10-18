using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class mic : MonoBehaviour
{
    public TextMeshProUGUI pitchText;
    public TextMeshProUGUI volumeText;

    private AudioSource audioSource;
    private float[] samples;
    private float[] spectrum;
    private const int sampleRate = 44100;
    private const int sampleCount = 1024;
    private const float refValue = 0.1f;
    private const float threshold = 0.02f;

    public point pt;

    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone detected");
            return;
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = Microphone.Start(null, true, 1, sampleRate);
        audioSource.loop = true;
        //audioSource.mute = true; // Prevent feedback
        while (!(Microphone.GetPosition(null) > 0)) { } // Wait until the recording has started
        audioSource.Play(); // Play the audio source

        samples = new float[sampleCount];
        spectrum = new float[sampleCount];
    }

    void Update()
    {
        GetMicrophoneData();
        AnalyzeSound();
    }

    void GetMicrophoneData()
    {
        audioSource.GetOutputData(samples, 0);
    }

    void AnalyzeSound()
    {
        float sum = 0;
        for (int i = 0; i < sampleCount; i++)
        {
            sum += samples[i] * samples[i]; // sum squared samples
        }

        float rmsValue = Mathf.Sqrt(sum / sampleCount); // rms = square root of average
        float dbValue = 20 * Mathf.Log10(rmsValue / refValue); // calculate dB
        if (dbValue < -160) dbValue = -160; // clamp it to -160dB min

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (int i = 0; i < sampleCount; i++)
        {
            if (!(spectrum[i] > maxV) || !(spectrum[i] > threshold))
                continue;
            maxV = spectrum[i];
            maxN = i; // maxN is the index of max
        }

        float pitchN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < sampleCount - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            pitchN += 0.5f * (dR * dR - dL * dL);
        }

        var pitchValue = pitchN * (sampleRate / 2) / sampleCount; // convert index to pitchuency

        pitchText.text = "Pitch: " + pitchValue.ToString("F0") + " Hz";
        volumeText.text = "Volume: " + dbValue.ToString("F1") + " dB";
        pt.CreateDBDataPoint(dbValue);
        pt.CreatePitchDataPoint(pitchValue);
    }
}

