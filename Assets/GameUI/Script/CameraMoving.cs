using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public float targetY = 0.0f;   // ��ǥ Y ��ġ
    public Rigidbody2D caps;
    private Vector3 initialPosition;  // �ʱ� ī�޶� ��ġ
    private float fallingSpeed;  // �������� �ӵ�
    public GameObject capsule;
    private bool capsuleRotated = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        Vector2 fallingVelocity = caps.velocity;
        fallingSpeed = fallingVelocity.magnitude; // fallingSpeed �ʱ�ȭ

        float newY = Mathf.MoveTowards(transform.position.y, targetY, fallingSpeed * Time.fixedDeltaTime);
        Vector3 newPosition = new Vector3(initialPosition.x, newY, initialPosition.z);
        transform.position = newPosition;

        if (capsule.transform.localPosition.y < -4 && !capsuleRotated)  // ���ϴ� Y ��ġ�� ����
        {
            Invoke("RotateCapsule", 1.0f);  // 1�� �� RotateCapsule �Լ� ����
            capsuleRotated = true;
        }
    }

    void RotateCapsule()
    {
        capsule.transform.rotation = Quaternion.Euler(0, 0, 0);  // ���ϴ� ȸ�� ������ ����
    }
}
