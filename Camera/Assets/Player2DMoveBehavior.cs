using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2DMoveBehavior : MonoBehaviour
{
    CharacterController characterController;
    private Vector2 velocity;
    private bool ViewRight;
    public float JumpPower;
    public float MoveSpeed;
    public float gravityConst = -20.0f; //플레이어 점프가 지구보다 너무 셈; 중력이 더 세야 자연스럽다.
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        velocity = new Vector2(0, 0);
        JumpPower = 10.0f;
    }

    void FixedUpdate()
    {
        velocity.y += gravityConst * Time.deltaTime;
        if (characterController.isGrounded && velocity.y < 0.0f)
        {
            velocity.y = 0;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right * (ViewRight ? -1 : 1), Vector3.up), 5 * Time.deltaTime);
        if (velocity != Vector2.zero)
        {
            characterController.Move(velocity * Time.deltaTime);
        }
        characterController.Move(-Vector3.forward * transform.position.z);
    }
    // Update is called once per frame
    void Update()
    {
        float inputY = Input.GetAxis("Jump");
        float inputX = Input.GetAxis("Horizontal");
        velocity.x = inputX * MoveSpeed;
        Debug.Log($"isGrounded(): {isGrounded()}, inputY > 0 : {inputY > 0}");
        if (isGrounded() && inputY > 0)
        {
            velocity.y = JumpPower * inputY;
        }
        if (inputX != 0)
        {
            ViewRight = inputX > 0;
        }
    }
    public bool isGrounded()
    {
        Ray ray = new Ray(transform.position - Vector3.up * characterController.height / 2, -Vector3.up);
        return characterController.isGrounded || Physics.Raycast(ray, 0.3f);
        //return characterController.isGroaunded;
    }
}
