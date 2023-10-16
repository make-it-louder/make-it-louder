using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveBehavior : MonoBehaviour
{
    private CharacterController characterController;
    private float angleX = 0.0f;
    private float velocityY = 0.0f;
    private Vector3 targetPosition;
    public float radius = 32f;
    private readonly float gravity = -9.8f;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        targetPosition.x = radius * Mathf.Cos(angleX);
        targetPosition.z = radius * Mathf.Sin(angleX);
    }

    private void FixedUpdate()
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0.0f;
        Move(direction);
    }
    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Jump");
        angleX = (angleX + 0.2f * inputH * Time.deltaTime) % (2 * Mathf.PI);
        targetPosition.x = radius * Mathf.Cos(angleX);
        targetPosition.z = radius * Mathf.Sin(angleX);
        Debug.Log(angleX);
        velocityY += gravity * Time.deltaTime;
        if (characterController.isGrounded && velocityY < 0.0f)
        {
            velocityY = 0;
        }
        if (characterController.isGrounded && inputV > 0.0f)
        {
            velocityY = 10.0f;
        }
        if (velocityY != 0)
        {
            Move(Vector3.up * velocityY * Time.deltaTime);
        }
    }
    public void Move(Vector3 direction)
    {
        Debug.DrawRay(transform.position, direction);
        characterController.Move(direction);
    }
}
