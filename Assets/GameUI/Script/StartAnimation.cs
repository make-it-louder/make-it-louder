using UnityEngine;
using UnityEngine.UI;

public class BirdFlyAnimation : MonoBehaviour
{
    public Button loginButton;
    public Transform targetPosition; // 오른쪽 대각선으로 이동할 위치
    public Transform cameraPosition; // 카메라의 위치 (새가 도달할 위치)
    public float speed = 1.0f;

    private bool startAnimation = false;
    private bool moveToCamera = false;

    private void Start()
    {
        loginButton.onClick.AddListener(StartFlyAnimation);
    }

    public void StartFlyAnimation()
    {
        startAnimation = true;
    }

    private void Update()
    {
        if (startAnimation)
        {
            if (!moveToCamera)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition.position) < 0.1f)
                {
                    moveToCamera = true;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, cameraPosition.position, speed * Time.deltaTime);
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f); // 새의 크기를 점점 늘립니다.

                if (Vector3.Distance(transform.position, cameraPosition.position) < 0.1f)
                {
                    startAnimation = false; // 애니메이션 종료
                }
            }
        }
    }
}
