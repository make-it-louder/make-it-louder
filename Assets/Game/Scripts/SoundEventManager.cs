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

    public Dictionary<int, INormalizedSoundInput> soundInputs;

    void Awake()
    {
        soundInputs = new Dictionary<int, INormalizedSoundInput>();
    }
    [PunRPC]
    void PhotonAddPublisher(int viewID)
    {
        if (soundInputs.ContainsKey(viewID))
        {
            return;
        }
        INormalizedSoundInput input = PhotonView.Find(viewID).GetComponentInChildren<INormalizedSoundInput>();
        if (input == null)
        {
            Debug.LogError($"Cannot find publisher with viewID={viewID}");
            return;
        }
        soundInputs.Add(viewID, input);
    }
    [PunRPC]
    void PhotonRemovePublisher(int viewID)
    {
        if (!soundInputs.ContainsKey(viewID))
        {
            return;
        }
        soundInputs.Remove(viewID);
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

    public float GetLocalDBAt(GameObject other)
    {
        if (other == null)
        {
            return 0f;
        }
        float DB = 0.0f;
        lock (soundInputs)
        {
            for (int i = 0;i < soundInputs.Count;i++)
            {
                INormalizedSoundInput soundInput = soundInputs[i];
                try
                {
                    float distance = Vector2.Distance(soundInput.gameObject.transform.position, other.transform.position);
                    if (distance < maxDistance)
                    {
                        float localDBEffect = soundInput.normalizedDB / (distance * distance);
                        DB += localDBEffect;
                    }
                }
                catch (MissingReferenceException)
                {
                    continue;
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