using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMove2D : MonoBehaviourPun
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    new BoxCollider2D collider;
    new SpriteRenderer renderer;
    Animator animator;

    public MicInputManager micInput;

    public float speed;
    public float jumpPower;
    float inputV;
    float inputH;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        renderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        animator = transform.Find("Renderer").GetComponent<Animator>();
        //rb.centerOfMass = rb.centerOfMass - new Vector2(0, 0.15f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputH != 0)
        {
            rb.velocity = new Vector2(inputH * speed, rb.velocity.y);
        }
        //Debug.Log($"inputV > 0 : {inputV > 0}, isGrounded(): {isGrounded()}");
        if (inputV > 0 && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
        }

        if (!isGrounded() && micInput.DB > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -3.5f));
        }

        if ((rb.velocity.x < -0.003f) != renderer.flipX)
        {
            renderer.flipX = rb.velocity.x < 0;
        }
        animator.SetFloat("hVelocity", Mathf.Abs(inputH));
        animator.SetFloat("vVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded());
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            inputV = Input.GetAxis("Jump");
            inputH = Input.GetAxis("Horizontal");
        }
    }
    bool isGrounded()
    {
        Vector3 feet = rb.transform.position + transform.up * transform.lossyScale.y * (collider.offset.y - collider.size.y / 2 - 0.01f) + transform.right * transform.lossyScale.x * collider.offset.x; ;
        Vector3 feetLeft = feet - transform.right * transform.lossyScale.x * (collider.size.x / 2) * 0.99f;
        Vector3 feetRight = feet + transform.right * transform.lossyScale.x * (collider.size.x / 2) * 0.99f;
        return (Physics2D.OverlapPoint(feetLeft) != null) || (Physics2D.OverlapPoint(feetRight) != null);
    }
}