using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public float knockbackStrength = 40f;  // 밀려나는 힘의 크기를 설정합니다.
    PlayerMove2D playerMove2D;
    IEnumerator ResetIgnoreInput()
    {
        yield return new WaitForSeconds(1f);  // 예를 들어, 0.5초 동안 입력을 무시합니다.
        playerMove2D.IgnoreInput = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // 플레이어와 충돌했는지 확인합니다.
        if (collider.gameObject.CompareTag("Player"))
        {
            // 플레이어의 Rigidbody2D 컴포넌트를 가져옵니다.
            Rigidbody2D playerRb = collider.gameObject.GetComponent<Rigidbody2D>();
            playerMove2D = collider.gameObject.GetComponent<PlayerMove2D>();
            playerMove2D.IgnoreInput = true;
            StartCoroutine(ResetIgnoreInput());

            // 밀려나는 방향과 크기를 계산합니다.
            Vector2 knockbackDirection = (playerRb.transform.position - transform.position).normalized * knockbackStrength;
            knockbackDirection.y = 5f;
            if (knockbackDirection.x <= 0) knockbackDirection.x = -knockbackStrength;
            else knockbackDirection.x = knockbackStrength;
            Debug.Log(knockbackDirection);

            // 플레이어에게 밀려나는 효과를 적용합니다.a
            playerRb.velocity = knockbackDirection;
        }
    }
}
