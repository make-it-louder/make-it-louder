using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeManager : MonoBehaviour
{
    public GameObject Window;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // "Player" 레이어가 적용된 오브젝트인지 확인
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 해당 오브젝트에 PhotonView 컴포넌트가 있는지 확인
            PhotonView photonView = collision.gameObject.GetComponent<PhotonView>();
            if (photonView != null)
            {
                // PhotonView.isMine이 true인지 확인
                if (photonView.IsMine)
                {
                    Window.SetActive(true);
                    // 여기에 필요한 로직을 구현
                    Debug.Log("Collision with Player layer and PhotonView is mine");
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // "Player" 레이어가 적용된 오브젝트인지 확인
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // 해당 오브젝트에 PhotonView 컴포넌트가 있는지 확인
            PhotonView photonView = collision.gameObject.GetComponent<PhotonView>();
            if (photonView != null)
            {
                // PhotonView.isMine이 true인지 확인
                if (photonView.IsMine)
                {
                    Window.SetActive(false);
                    // 여기에 필요한 로직을 구현
                    Debug.Log("Collision with Player layer and PhotonView is mine");
                }
            }
        }
    }
}
