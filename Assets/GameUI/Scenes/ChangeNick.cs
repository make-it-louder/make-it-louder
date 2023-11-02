using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeNick : MonoBehaviour
{
    public TMP_InputField cn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void UpdateNick()
    {
        await RecordManager.Instance.UpdateUsername(cn.text);
    }
}
