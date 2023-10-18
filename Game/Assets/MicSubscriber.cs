using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicSubscriber : MonoBehaviour
{
    [SerializeField]
    MicInputManager input;
    public float minDB = -160;
    public float maxDB = 1000;

    // Start is called before the first frame update
    void Start()
    {

        if (input == null)
        {
            setScale(0);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null)
        {
            return;
        }
        setScale(getDBRatio(input.DB) * 0.25f);
    }
    void setScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }
    float getDBRatio(float db)
    {
        return (db - minDB) / (maxDB - minDB);
    }
}