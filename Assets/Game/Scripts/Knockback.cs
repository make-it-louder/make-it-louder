using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackStrength = 100f;  // 밀려나는 힘의 크기를 설정합니다.

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌");

        // 플레이어와 충돌했는지 확인합니다.
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어!!!플레이어!!!플레이어!!!플레이어!!!플레이어!!!플레이어!!!플레이어!!!플레이어!!!플레이어!!!플레이어!!!플레이어!!!플레이어!!!");
            Debug.Log(collision.gameObject.name);
            // 플레이어의 Rigidbody2D 컴포넌트를 가져옵니다.
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

            // 밀려나는 방향과 크기를 계산합니다.
            Vector2 knockbackDirection = (playerRb.transform.position - transform.position).normalized * knockbackStrength;
            Debug.Log(knockbackDirection);

            // 플레이어에게 밀려나는 효과를 적용합니다.a
            playerRb.velocity = new Vector2(knockbackDirection.x, knockbackDirection.y);
        }
    }
}
