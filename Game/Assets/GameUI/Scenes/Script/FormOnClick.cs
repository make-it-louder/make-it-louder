using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormOnClick : MonoBehaviour
{
    public FixedUIController onClickController;
    public GameObject audioSettingsForm;
    public GameObject VideoSettingsForm;
    // Start is called before the first frame update
    void Start()
    {
        audioSettingsForm = GameObject.Find("AudioSettingsForm");
        VideoSettingsForm = GameObject.Find("VideoSettingsForm");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeFormHandler()
    {

    }

}
