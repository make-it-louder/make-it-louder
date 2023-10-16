using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelOnGaugeChange : MonoBehaviour, OnGaugeChangeBehavior
{
    public SpeakerInputManager speakerInputManager;
    private Rigidbody rb;
    public float rotateSpeed;
    public float initialRotation;

    float angularVelocity;
    float angularRotation;
    public void OnGaugeChange(float gauge)
    {
        angularVelocity = 2 * (gauge - 0.5f);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (speakerInputManager == null)
        {
            Debug.LogError("SpeakerInputManager is not given");
        }
        speakerInputManager.AddListener(gameObject.GetComponent<WheelOnGaugeChange>());
        rb = GetComponent<Rigidbody>();
        initialRotation = Random.value * 360.0f;
        rotateSpeed = Random.value * 5;
        angularRotation = initialRotation;
    }
    void FixedUpdate()
    {
        angularRotation += angularVelocity * rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, angularRotation * 360.0f);
    }
}
