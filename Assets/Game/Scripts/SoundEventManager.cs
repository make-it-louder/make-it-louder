using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundEventManager : MonoBehaviourPun
{
    [Tooltip("Ignore voice beyond maxDistance")]
    public float maxDistance;
    [SerializeField]
    private List<INormalizedSoundInput> soundInputs;

    void Awake()
    {
        soundInputs = new List<INormalizedSoundInput>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    [PunRPC]
    public void SyncAddPublisher(int viewID)
    {
        INormalizedSoundInput other = PhotonView.Find(viewID).gameObject.GetComponent<INormalizedSoundInput>();
        soundInputs.Add(other);
    }

    [PunRPC]
    public void SyncRemovePublisher(int viewID)
    {
        INormalizedSoundInput other = PhotonView.Find(viewID).gameObject.GetComponent<INormalizedSoundInput>();
        soundInputs.Remove(other);
    }

    public void AddPublisher(INormalizedSoundInput other)
    {
        photonView.RPC("SyncAddPublisher", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID);
    }

    public void RemovePublisher(INormalizedSoundInput other)
    {
        photonView.RPC("SyncRemovePublisher", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID);
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
            else
            {
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