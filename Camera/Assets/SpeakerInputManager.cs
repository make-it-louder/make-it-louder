using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerInputManager : MonoBehaviour
{
    [SerializeField]
    private float gauge;
    public Slider slider;
    public List<OnGaugeChangeBehavior> onGaugeChangeBehavior;
    // Start is called before the first frame update
    void Awake()
    {
        onGaugeChangeBehavior = new List<OnGaugeChangeBehavior>();
    }
    void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider is not given");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void AddListener(OnGaugeChangeBehavior listener)
    {
        onGaugeChangeBehavior.Add(listener);
    }
    public void OnGaugeChange()
    {
        gauge = slider.value;
        foreach (OnGaugeChangeBehavior listener in onGaugeChangeBehavior) {
            listener.OnGaugeChange(gauge);
        }
    }
}
