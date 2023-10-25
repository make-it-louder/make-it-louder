using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEventManager : MonoBehaviour
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

    public void AddPublisher(INormalizedSoundInput other)
    {
        soundInputs.Add(other);
    }

    public void DeletePublisher(INormalizedSoundInput other)
    {
        soundInputs.Remove(other);
    }
    public float GetLocalDBAt(GameObject other)
    {
        float DB = 0.0f;
        foreach (INormalizedSoundInput soundInput in soundInputs)
        {
            float distance = Vector2.Distance(soundInput.transform.position, other.transform.position);
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
    GameObject gameObject;
    public Transform transform
    {
        get
        {
            return gameObject?.transform;
        }
    }
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