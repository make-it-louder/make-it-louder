using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public float targetY = 0.0f;   // 목표 Y 위치
    public Rigidbody2D caps;
    private Vector3 initialPosition;  // 초기 카메라 위치
    private float fallingSpeed;  // 떨어지는 속도
    public GameObject capsule;
    private bool capsuleRotated = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector2 fallingVelocity = caps.velocity;
        fallingSpeed = fallingVelocity.magnitude; // fallingSpeed 초기화

        float newY = Mathf.MoveTowards(transform.position.y, targetY, fallingSpeed * Time.fixedDeltaTime);
        Vector3 newPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
        transform.position = newPosition;

        if (capsule.transform.localPosition.y < -4 && !capsuleRotated)  // 원하는 Y 위치로 변경
        {
            Invoke("RotateCapsule", 1.0f);  // 1초 후 RotateCapsule 함수 실행
            capsuleRotated = true;
        }
    }

    void RotateCapsule()
    {
        capsule.transform.rotation = Quaternion.Euler(0, 0, 0);  // 원하는 회전 각도로 설정
    }
}
