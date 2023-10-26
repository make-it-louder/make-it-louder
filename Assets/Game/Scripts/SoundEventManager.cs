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
        if (photonEvent.Code == (byte)PunEventCode.Code.ModifySoundEvent)
        {
            object[] data = (object[])photonEvent.CustomData;
            var dto = new PunEventCode.ModifySoundEventDTO(data);
            Debug.Log($"Event Received: {photonEvent}");
            Debug.Log($"OnEvent Received: dto={dto}");
            switch (dto.code)
            {
                case PunEventCode.ModifySoundEventDTO.Code.add:
                    SyncAddPublisher(dto.viewID);
                    break;
                case PunEventCode.ModifySoundEventDTO.Code.remove:
                    SyncRemovePublisher(dto.viewID);
                    break;
                case PunEventCode.ModifySoundEventDTO.Code.addAndGet:
                    SyncAddPublisher(dto.viewID);
                    if (PhotonNetwork.IsMasterClient)
                    {
                        byte code = (byte)PunEventCode.Code.GetSoundEventResponse;
                        var Serializable = new PunEventCode.GetSoundEventResponseDTO(soundInputs);
                        PunEventSender.Send(code, Serializable, new int[] { photonEvent.Sender }, SendOptions.SendReliable);
                    }
                    break;
            }
        }
        if (photonEvent.Code == (byte)PunEventCode.Code.GetSoundEventResponse)
        {
            var dto = new PunEventCode.GetSoundEventResponseDTO((object[])photonEvent.CustomData);
            Debug.Log($"Event Received: {photonEvent}");
            Debug.Log($"OnEvent Received: dto={dto}");
            List<int> viewIDs = dto.list;
            SyncSetPublisher(viewIDs.ToArray());
        }
    }

    public void SyncAddPublisher(int viewID)
    {
        INormalizedSoundInput other = PhotonView.Find(viewID).gameObject.GetComponentInChildren<INormalizedSoundInput>();
        if (!soundInputs.Contains(other))
        {
            lock (soundInputs)
            {
                soundInputs.Add(other);
            }
        }
        soundInputsDebug.Add(other.gameObject);
    }
    public void SyncSetPublisher(int[] viewIDs)
    {
        lock (soundInputs)
        {
            soundInputs = new List<INormalizedSoundInput>();
            soundInputsDebug = new List<GameObject>();
            foreach (var viewID in viewIDs)
            {
                SyncAddPublisher(viewID);
            }
        }
    }

    public void SyncRemovePublisher(int viewID)
    {
        INormalizedSoundInput other = PhotonView.Find(viewID).gameObject.GetComponentInChildren<INormalizedSoundInput>();
        if (soundInputs.Contains(other))
        {
            lock (soundInputs)
            {
                soundInputs.Remove(other);
            }
        }
        soundInputsDebug.Remove(other.gameObject);
    }
    

    public void AddPublisher(INormalizedSoundInput other)
    {
        int viewID = other.gameObject.transform.parent.GetComponent<PhotonView>().ViewID;
        var dto = new PunEventCode.ModifySoundEventDTO(PunEventCode.ModifySoundEventDTO.Code.add, viewID);
        PunEventSender.SendToAll((byte)PunEventCode.Code.ModifySoundEvent, dto);
    }

    public void RemovePublisher(INormalizedSoundInput other)
    {
        int viewID = other.gameObject.transform.parent.GetComponent<PhotonView>().ViewID;
        var dto = new PunEventCode.ModifySoundEventDTO(PunEventCode.ModifySoundEventDTO.Code.remove, viewID);
        PunEventSender.SendToAll((byte)PunEventCode.Code.ModifySoundEvent, dto);
    }

    public void AddAndSyncPublisher(INormalizedSoundInput other)
    {
        int viewID = other.gameObject.transform.parent.GetComponent<PhotonView>().ViewID;
        var dto = new PunEventCode.ModifySoundEventDTO(PunEventCode.ModifySoundEventDTO.Code.addAndGet, viewID);
        PunEventSender.SendToOthers((byte)PunEventCode.Code.ModifySoundEvent, dto);
    }
    public float GetLocalDBAt(GameObject other)
    {
        float DB = 0.0f;
        lock (soundInputs)
        {
            foreach (INormalizedSoundInput soundInput in soundInputs)
            {
                if (soundInput == null)
                {
                    RemovePublisher(soundInput);
                    continue;
                }
                float distance = Vector2.Distance(soundInput.gameObject.transform.position, other.transform.position);
                if (distance < maxDistance)
                {
                    float localDBEffect = soundInput.normalizedDB / (distance * distance);
                    DB += localDBEffect;
                }
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