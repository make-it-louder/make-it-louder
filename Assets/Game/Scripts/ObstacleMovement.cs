using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public Transform targetChild; // 자식 오브젝트를 여기에 드래그 앤 드롭합니다.
    private Vector3 startPosition;
    private Vector3 targetPosition;
    public float speed = 1.0f; // 이동 속도

    void Start()
    {
        if (targetChild == null)
        {
            Debug.LogError("Target child is not assigned!");
            return;
        }

        startPosition = transform.position; // 오브젝트의 시작 위치 저장
        targetPosition = targetChild.position; // 자식 오브젝트의 위치를 목표 위치로 설정
        targetChild.GetComponent<SpriteRenderer>().enabled = false;
    }

    void Update()
    {
        // 시간에 따라 왕복 운동을 생성
        float t = (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f;

        transform.position = Vector2.Lerp(startPosition, targetPosition, t);

        if (transform.position.x >= targetPosition.x-0.1f)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        if (transform.position.x <= startPosition.x + 0.1f)
        {
            transform.localScale = new Vector2(1, 1);
        }

    }
}
