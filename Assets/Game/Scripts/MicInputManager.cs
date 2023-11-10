using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using System;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Audio;

public class MicInputManager : MonoBehaviour, INormalizedSoundInput
{

    public float minDB = -15.0f;
    public float maxDB = 5.0f;
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
            throw new NotSupportedException("Pitch not supported; Please uncomment the comment in this file to use  it");
        }
        private set
        {
            pitch = value;
        }
    }
    private float pitch;
    public float normalizedDB
    {
        get
        {
            return Mathf.Clamp((DB - minDB) / (maxDB - minDB), 0.0f, 1.0f);
        }
    }
    public float DB
    {
        get
        {
            return db;
        }
    }

    private float db;
    [SerializeField]
    private SoundEventManager soundEventManager;
    public SoundEventManager SoundEventManager
    {
        get
        {
            return soundEventManager;
        }
        set
        {
            if (soundEventManager == value)
            {
                return;
            }
            if (soundEventManager != null)
            {
                soundEventManager.RemovePublisher(this);
            }
            soundEventManager = value;
            soundEventManager.AddPublisher(this);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (!PhotonNetwork.IsConnected || PhotonNetwork.OfflineMode)
        {
            if (Microphone.devices.Length == 0)
            {
                Debug.LogError("No microphone detected");
                return;
            }
            audioSource.clip = Microphone.Start(null, true, 1, sampleRate);
            audioSource.loop = true;
            //audioSource.mute = true; // Prevent feedback
            while (!(Microphone.GetPosition(null) > 0)) { } // Wait until the recording has started
            audioSource.Play(); // Play the audio source
        }
        if (soundEventManager != null)
        {
            soundEventManager.AddPublisher(this);
        }
        PhotonView pv = GetComponent<PhotonView>();
        if (pv != null && pv.IsMine)
        {
            AudioMixer mixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
            if (mixer == null)
            {
                Debug.LogError("Cannot find Mixer at Resources/Auudio/AudioMixer");
            }
            AudioMixerGroup[] group = mixer.FindMatchingGroups("MyVoice");
            if (group.Length == 0)
            {
                Debug.LogError("Cannot find AudioMixerGroup MyVoice");
            }
            if (group.Length >= 2)
            {
                Debug.LogError("AudioMixerGroup MyVoice is ambiguous!");
            }
            audioSource.outputAudioMixerGroup = group[0];
        }

        samples = new float[sampleCount];
        spectrum = new float[sampleCount];

        Pitch = 0;
        db = 0;
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
        float dbValue = 10 * Mathf.Log10(rmsValue / refValue) - 10 * Mathf.Log10(audioSource.volume); // calculate dB
        if (dbValue < -80) dbValue = -80; // clamp it to -80dB min
        /*
         * The code below had been commented out becasue the game doesn't use it.
         * To get a pitch value, please uncomment it.
        */
        /*
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
        */
        db = dbValue;
    }
    void OnDestroy()
    {
        if (soundEventManager != null)
        {
            soundEventManager.RemovePublisher(this);
        }
    }
}
