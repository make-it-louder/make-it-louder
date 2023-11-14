using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip lobby;
    public AudioClip forest;
    public AudioClip cliff;
    public float lobbyVol = 0.08f;
    public float forestVol = 0.2f;
    public float cliffVol = 0.2f;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playLobby();
    }

    public void playForest()
    {
        audioSource.clip = forest;
        audioSource.Play();
    }


    public void playCliff()
    {
        audioSource.clip = cliff;
        audioSource.Play();
    }

    public void playLobby()
    {
        audioSource.clip = lobby;
        audioSource.Play();
    }
    public bool isPlay(AudioClip clip)
    {
        return audioSource.clip == clip;
    }
}
