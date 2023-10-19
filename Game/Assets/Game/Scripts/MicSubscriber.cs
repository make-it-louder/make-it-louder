using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicSubscriber : MonoBehaviour
{
    [SerializeField]
    MicInputManager input;
    public float minDB = -160;
    public float maxDB = 1000;
    public float scaleFactor = 1.0f;

    public Color normalColor = Color.green;
    public Color bigSoundColor = Color.red;
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {

        if (input == null)
        {
            setScale(0);
            return;
        }
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null)
        {
            return;
        }
        if (input.DB < minDB)
        {
            setScale(0);
            setColor(normalColor);
        }
        else if (input.DB > maxDB)
        {
            setScale(1);
            setColor(bigSoundColor);
        }
        else
        {
            setScale(getDBRatio(input.DB));
            setColor(normalColor);
        }
    }
    void setScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale) * scaleFactor;
    }
    float getDBRatio(float db)
    {
        return (db - minDB) / (maxDB - minDB);
    }
    void setColor(Color color)
    {
        renderer.color = color;
    }
}