using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System.Linq;

public class SoundEventManager : MonoBehaviourPun, IOnEventCallback
{
    [Tooltip("Ignore voice beyond maxDistance")]
    public float maxDistance;
    
    public List<INormalizedSoundInput> soundInputs;
    public List<GameObject> soundInputsDebug;

    void Awake()
    {
        soundInputs = new List<INormalizedSoundInput>();
        soundInputsDebug = new List<GameObject>();
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        Debug.Log($"Event Received: {photonEvent}");
        if (photonEvent.Code == PunEventCode.ModifySoundEvent)
        {
            var dto = new PunEventCode.ModifySoundEventDTO((object[])photonEvent.CustomData);
            Debug.Log($"OnEvent Received: dto={dto}");
            if (dto.add)
            {
                SyncAddPublisher(dto.viewID);
            }
            else
            {
                SyncRemovePublisher(dto.viewID);
            }
        }
    }

    public void SyncAddPublisher(int viewID)
    {
        INormalizedSoundInput other = PhotonView.Find(viewID).gameObject.GetComponentInChildren<INormalizedSoundInput>();
        soundInputs.Add(other);
        soundInputsDebug.Add(other.gameObject);
    }

    public void SyncRemovePublisher(int viewID)
    {
        INormalizedSoundInput other = PhotonView.Find(viewID).gameObject.GetComponentInChildren<INormalizedSoundInput>();
        soundInputs.Remove(other);
        soundInputsDebug.Remove(other.gameObject);
    }
    

    public void AddPublisher(INormalizedSoundInput other)
    {
        int viewID = other.gameObject.transform.parent.GetComponent<PhotonView>().ViewID;
        var dto = new PunEventCode.ModifySoundEventDTO(true, viewID);
        PunEventSender.SendToAll(PunEventCode.ModifySoundEvent, dto);
    }

    public void RemovePublisher(INormalizedSoundInput other)
    {
        int viewID = other.gameObject.transform.parent.GetComponent<PhotonView>().ViewID;
        var dto = new PunEventCode.ModifySoundEventDTO(false, viewID);
        PunEventSender.SendToAll(PunEventCode.ModifySoundEvent, dto);
    }
    public float GetLocalDBAt(GameObject other)
    {
        float DB = 0.0f;
        foreach (INormalizedSoundInput soundInput in soundInputs)
        {
            float distance = Vector2.Distance(soundInput.gameObject.transform.position, other.transform.position);
            if (distance < maxDistance)
            {
                float localDBEffect = soundInput.normalizedDB / (distance * distance);
                DB += localDBEffect;
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
            return manager.GetLocalDBAt(gameObject);
        }
    }
}