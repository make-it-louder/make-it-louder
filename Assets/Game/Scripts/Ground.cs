using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerMove2D move;
    void Start()
    {
        move = GetComponentInParent<PlayerMove2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 'Ground' 레이어에 속하는지 확인합니다.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // 충돌 접촉점들 중 어느 것이든 오브젝트의 아래쪽에 있는지 확인합니다.
            foreach (ContactPoint2D point in collision.contacts)
            {
                // 여기서 Vector2.up은 오브젝트의 로컬 '위' 방향을 의미합니다.
                // 이 방향이 접촉점의 노멀과 반대방향(즉, 오브젝트가 바닥 위에 있음)인지 확인합니다.
                if (Vector2.Dot(point.normal, Vector2.up) > 0.5)
                {
                    // 접촉점의 노멀이 위를 가리키면, 그것은 오브젝트가 바닥에 있다는 것을 의미합니다.
                    move.isGrounded = true;
                    break; // 하나의 접촉점만 바닥과의 충돌로 충분하므로 루프를 나갑니다.
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 오브젝트가 바닥에서 떨어졌는지 확인하기 위해 'Ground' 레이어에 속한 오브젝트와의 충돌이 끝났는지 확인합니다.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            move.isGrounded = false;
        }
    }
}
