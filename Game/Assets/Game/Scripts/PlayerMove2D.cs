using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove2D : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    new BoxCollider2D collider;
    new SpriteRenderer renderer;
    Animator animator;

    public float speed; 
    public float jumpPower;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        renderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        animator = transform.Find("Renderer").GetComponent<Animator>();
        //rb.centerOfMass = rb.centerOfMass - new Vector2(0, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        float inputV = Input.GetAxis("Jump");
        float inputH = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(inputH * speed, rb.velocity.y);
        if ((inputH != 0) && (inputH < 0) != renderer.flipX)
        {
            renderer.flipX = inputH < 0;
        }
        //Debug.Log($"inputV > 0 : {inputV > 0}, isGrounded(): {isGrounded()}");
        if (inputV > 0 && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
        }
        animator.SetFloat("hVelocity", Mathf.Abs(inputH));
        animator.SetFloat("vVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded());
    }
    bool isGrounded()
    {
        Vector3 feet = rb.transform.position + transform.up * transform.lossyScale.y * (collider.offset.y - collider.size.y / 2 - 0.01f);
        Vector3 feetLeft = feet - transform.right * transform.lossyScale.x * (collider.offset.x + collider.size.x / 2) * 0.9f;
        Vector3 feetRight = feet + transform.right * transform.lossyScale.x * (collider.offset.x + collider.size.x / 2) * 0.9f;
        RaycastHit2D hitLeft = Physics2D.Raycast(feetLeft, -transform.up, 0.2f);
        RaycastHit2D hitRight = Physics2D.Raycast(feetRight, -transform.up, 0.2f);
        Debug.DrawRay(feetLeft, -transform.up * 0.2f);
        Debug.DrawRay(feetRight, -transform.up * 0.2f);
        Debug.Log($"{hitLeft.collider} {hitRight.collider}");
        return (hitLeft.collider != null) || (hitRight.collider != null);
    }
}
