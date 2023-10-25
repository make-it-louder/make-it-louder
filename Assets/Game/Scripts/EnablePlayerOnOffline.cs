using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnablePlayerOnOffline : MonoBehaviour
{
    [SerializeField]
    private GameObject[] toEnable;
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            foreach (var t in toEnable)
            {
                t.SetActive(true);
            }
        }
    }
}
