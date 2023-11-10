using UnityEngine;
using Photon.Pun;

public class ObstacleMover : MonoBehaviourPun
{
    [SerializeField]
    SoundEventManager soundManager;
    SoundSubscriber input;
    public Transform targetChild; // 자식 오브젝트를 여기에 드래그 앤 드롭합니다.
    private Vector3 startPosition;
    private Vector3 targetPosition;
    public float speed = 1.0f; // 이동 속도
    private float timer = 0.0f;
    public float velocityRate = 1.0f;
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
        input = soundManager.Subscribe(this.gameObject);
    }


    void Update()
    {

        if (!photonView.IsMine)
        {
            return;
        }
        if (timer > 1000) timer -= Mathf.Floor(timer / (2 * Mathf.PI)) * (2 * Mathf.PI);

        if (input.normalizedDB > 0.01f)
        {
            timer += speed * Time.deltaTime * velocityRate;
        }
        else
        {
            timer += speed * Time.deltaTime;
        }
        // 시간에 따라 왕복 운동을 생성

        float t = (Mathf.Sin(timer) + 1.0f) / 2.0f;

        // Lerp 함수를 사용하지 않고 수동으로 보간
        float newX = startPosition.x * (1 - t) + targetPosition.x * t;
        float newY = startPosition.y * (1 - t) + targetPosition.y * t;

        // transform.position 업데이트
        transform.position = new Vector2(newX, newY);

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
