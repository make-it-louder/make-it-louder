using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundBlockController : MonoBehaviour
{
    [SerializeField]
    MicInputManager input;

    Rigidbody2D rb;

    private float maxY;
    private float minY;
    private float curY;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxY = transform.position.y + 3;
        minY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        moveUp();
    }

    void moveUp()
    {
        if (input.DB > -5)
        {
            curY = transform.position.y;
            Vector2 newPosition = rb.position + new Vector2(0, 0.1f);
            if (newPosition.y > maxY)
            {
                newPosition.y = maxY;
            }
            rb.MovePosition(newPosition);

        }
        else
        {
            Vector2 newPosition = rb.position + new Vector2(0, -0.02f);
            if (newPosition.y < minY)
            {
                newPosition.y = minY;
            }
            rb.MovePosition(newPosition);
        }
    }
}
