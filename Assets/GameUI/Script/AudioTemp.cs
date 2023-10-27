using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTemp : MonoBehaviour
{
    public static AudioTemp instance;
    public AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("hi");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // BGM ���� ����
        audioSource.loop = true;

        // BGM ���
        audioSource.Play();
        Debug.Log("bye");
    }

}
