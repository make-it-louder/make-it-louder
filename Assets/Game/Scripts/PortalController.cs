using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{

    public int targetIdx = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 만약 충돌한 객체가 플레이어라면
        if (other.CompareTag("Player"))
        {
            // "targetPos" 태그를 가진 오브젝트를 찾음
            GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("targetPos");

            if (targetObjects.Length > 0)
            {
                // 첫 번째 "targetPos" 오브젝트의 위치로 플레이어를 이동
                Transform targetTransform = targetObjects[targetIdx].transform;
                Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();

                if (playerRigidbody != null)
                {
                    // 이동할 때 속도를 초기화하거나 다른 동작을 추가할 수 있습니다.
                    playerRigidbody.velocity = Vector2.zero;
                    playerRigidbody.angularVelocity = 0f;

                    // 플레이어의 위치를 "targetPos" 오브젝트의 위치로 설정
                    other.transform.position = targetTransform.position;
                }
            }
        }
    }
}