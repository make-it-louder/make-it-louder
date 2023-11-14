using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{

    SoundSubscriber soundSubscriber;
    Rigidbody2D rb;

    [SerializeField]
    SoundEventManager soundManager;

    private float rotateSpeed = 0.2f;
    public bool reverse;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soundSubscriber = soundManager.Subscribe(gameObject);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(soundSubscriber.normalizedDB);
        if (soundSubscriber.normalizedDB > 0.001f)
        {
            rotateSpeed = Mathf.Clamp(soundSubscriber.normalizedDB * 2,0.2f,2.0f);
        }
        else
        {
            rotateSpeed = 0.2f;
        }

        if (!reverse)
        {
            transform.Rotate(0, 0, rotateSpeed);
        }
        else
        {
            transform.Rotate(0, 0, -rotateSpeed);
        }
        
    }
}
