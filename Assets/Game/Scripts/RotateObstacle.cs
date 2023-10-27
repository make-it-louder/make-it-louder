using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObstacle : MonoBehaviour
{

    Rigidbody2D rb;

    public float rotateSpeed = 0.2f;
    public bool reverse;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
