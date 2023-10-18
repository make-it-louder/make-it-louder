using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

public class MicInputManager : MonoBehaviour
{
    private AudioSource audioSource;
    private float[] samples;
    private float[] spectrum;
    private const int sampleRate = 44100;
    private const int sampleCount = 1024;
    private const float refValue = 0.1f;
    private const float threshold = 0.02f;

    public float Pitch
    {
        get
        {
            return pitch;
        }
        set
        {
            pitch = value;
        }
    }
    private float pitch;
    public float DB
    {
        get
        {
            return db;
        }
        set
        {
            db = value;
        }
    }
    private float db;
    
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

        Pitch = 0;
        DB = 0;
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

        Pitch = pitchN * (sampleRate / 2) / sampleCount; // convert index to pitchuency
        DB = dbValue;
    }
}
