using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform targetObject; // 따라갈 타겟 오브젝트
    public float positionLerpSpeed = 0.1f; // 위치를 따라가는 속도 (0에서 1 사이의 값)
    public float rotationLerpSpeed = 0.1f; // 회전을 따라가는 속도 (0에서 1 사이의 값)

    void Update()
    {
        // 위치를 따라갑니다.
        transform.position = Vector3.Lerp(transform.position, targetObject.position, positionLerpSpeed);

        // 회전을 따라갑니다.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetObject.rotation, rotationLerpSpeed);
    }
}