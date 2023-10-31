using UnityEngine;

public class BounceOnCollision : MonoBehaviour
{
    public float bounceForce = 10f; // 튕겨나가는 힘의 정도

    void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null) // 충돌한 GameObject에 Rigidbody2D가 있는지 확인
        {
            Vector2 direction = collision.transform.position - transform.position; // 튕겨나가는 방향 계산
            direction.Normalize(); // 방향 벡터 정규화

            rb.AddForce(direction * bounceForce, ForceMode2D.Impulse); // 힘을 추가하여 튕겨나가게 함
        }
    }
}
