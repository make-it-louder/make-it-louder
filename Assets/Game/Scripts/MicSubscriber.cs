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
    private new SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (input == null)
        {
            input = transform.parent.GetComponentInChildren<MicInputManager>();
            if (input == null)
            {
                return;
            }
        }
        if (input.normalizedDB == 1.0f)
        {
            setScale(1);
            setColor(bigSoundColor);
        }
        else
        {
            setScale(input.normalizedDB);
            setColor(normalColor);
        }
    }
    void setScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale) * scaleFactor;
    }
    void setColor(Color color)
    {
        renderer.color = color;
    }
}