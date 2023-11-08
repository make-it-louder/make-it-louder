using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;

public class SoundEventManager : MonoBehaviourPun
{
    [Tooltip("Ignore voice beyond maxDistance")]
    public float maxDistance;

    private Dictionary<int, INormalizedSoundInput> soundInputs;
    private Dictionary<int, AudioSource> soundSources;
    private Dictionary<int, float> micVoiceInputSettings;
    private INormalizedSoundInput mine;
    void Awake()
    {
        soundInputs = new Dictionary<int, INormalizedSoundInput>();
        soundSources = new Dictionary<int, AudioSource>();
        micVoiceInputSettings = new Dictionary<int, float>();
    }
    [PunRPC]
    void PhotonAddPublisher(int viewID)
    {
        if (soundInputs.ContainsKey(viewID))
        {
            return;
        }
        PhotonView pv = PhotonView.Find(viewID);
        INormalizedSoundInput input = pv.GetComponentInChildren<INormalizedSoundInput>();
        if (input == null)
        {
            Debug.LogError($"Cannot find publisher with viewID={viewID}");
            return;
        }
        AudioSource audioSource = pv.GetComponentInChildren<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError($"Cannot find AudioSource with viewID={viewID}");
            return;
        }
        soundInputs.Add(viewID, input);
        soundSources.Add(viewID, audioSource);
    }
    [PunRPC]
    void PhotonRemovePublisher(int viewID)
    {
        if (!soundInputs.ContainsKey(viewID))
        {
            return;
        }
        soundInputs.Remove(viewID);
        soundSources.Remove(viewID);
    }

    [PunRPC]
    void PhotonSetMicVolume(int viewID, float volume)
    {
        micVoiceInputSettings[viewID] = volume;
        if(soundSources.ContainsKey(viewID)){
            soundSources[viewID].volume = volume;
        }
    }
    public void AddPublisher(INormalizedSoundInput other)
    {
        int? ViewID = other.gameObject?.GetComponent<PhotonView>()?.ViewID;
        if (ViewID == null)
        {
            Debug.LogError($"AddPublisher: the publisher you want to add has no ViewID in the GameObject(object={other}, gameObject={other.gameObject}");
            return;
        }
        photonView.RPC("PhotonAddPublisher", RpcTarget.AllBuffered, ViewID.Value);
        mine = other;
    }

    public void RemovePublisher(INormalizedSoundInput other)
    {
        int? ViewID = other.gameObject?.GetComponent<PhotonView>()?.ViewID;
        if (ViewID == null)
        {
            Debug.LogError("RemovePublisher: the publisher you want to add has no ViewID in the GameObject");
            return;
        }
        photonView.RPC("PhotonRemovePublisher", RpcTarget.AllBuffered, ViewID.Value);
    }

    public void SetMicVolume(float newVolume)
    {
        if (mine == null)
        {
            return;
        }
        int? ViewID = mine?.gameObject?.GetComponent<PhotonView>()?.ViewID;
        if (ViewID == null)
        {
            Debug.LogError("RemovePublisher: the publisher you want to add has no ViewID in the GameObject");
            return;
        }
        photonView.RPC("PhotonSetMicVolume", RpcTarget.AllBuffered, ViewID.Value, newVolume);
    }

    public float GetLocalDBAt(GameObject other)
    {
        if (other == null)
        {
            return 0f;
        }
        float DB = 0.0f;

        foreach (int i in soundInputs.Keys)
        {
            INormalizedSoundInput soundInput = soundInputs[i];
            if (soundInput == null)
            {
                continue;
            }
            try
            {
                float distance = Vector2.Distance(soundInput.gameObject.transform.position, other.transform.position);
                if (distance < maxDistance)
                {
                    float localDBEffect = soundInput.normalizedDB / (distance * distance);
                    DB = Mathf.Max(DB, localDBEffect);
                }
            }
            catch (MissingReferenceException)
            {
                continue;
            }
        }
        DB = Mathf.Clamp(DB, 0.0f, 1.0f);
        return DB;
    }
    public SoundSubscriber Subscribe(GameObject other)
    {
        return new SoundSubscriber(this, other);
    }
}

public class SoundSubscriber : INormalizedSoundInput
{
    SoundEventManager manager;
    public GameObject gameObject{ get; set; }
    public SoundSubscriber(SoundEventManager manager, GameObject gameObject)
    {
        this.manager = manager;
        this.gameObject = gameObject;
    }
    public float normalizedDB
    {
        get
        {
            if (gameObject == null)
            {
                return 0.0f;
            }
            return manager.GetLocalDBAt(gameObject);
        }
    }

}