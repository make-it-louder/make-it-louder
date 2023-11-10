using UltimateClean;
using UnityEngine;

public class FadeInObject : MonoBehaviour
{
    private Transform cameraTransform; // 카메라의 Transform 컴포넌트
    public float activationHeight = 10.0f; // 오브젝트가 활성화되기 시작하는 Y 높이
    public float fadeDuration = 2.0f; // 완전히 활성화되는데 걸리는 시간 (초)
    private SpriteRenderer spriteRenderer; // 이 오브젝트의 SpriteRenderer 컴포넌트
    private float currentFadeTime = 0f; // 현재 페이드 진행 시간
    public AudioClip forest;
    public AudioClip cliff;
    public BackgroundMusicController backgroundMusicController;
    void Start()
    {
        // SpriteRenderer 컴포넌트 참조
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 시작 시 오브젝트를 투명하게 설정
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
        cameraTransform = Camera.main.transform;
        backgroundMusicController = GameObject.FindObjectOfType<BackgroundMusicController>();
        backgroundMusicController.playForest();
    }

    void Update()
    {
        // 카메라의 Y 높이가 활성화 높이를 넘었는지 확인
        if (cameraTransform.position.y >= activationHeight)
        {
            // 현재 페이드 시간을 증가시키고, 그에 따라 투명도를 계산
            currentFadeTime += Time.deltaTime;
            currentFadeTime = Mathf.Min(currentFadeTime, fadeDuration); // fadeDuration을 초과하지 않도록 함
            if(backgroundMusicController.isPlay(forest))
            {
                backgroundMusicController.playCliff();
            }
        }

        else
        {
            // 카메라가 활성화 높이 아래로 내려갔을 때 투명도를 서서히 감소
            currentFadeTime -= Time.deltaTime;
            if (backgroundMusicController.isPlay(cliff))
            {
                backgroundMusicController.playForest();
            }
        }

        // currentFadeTime을 사용하여 투명도 계산
        currentFadeTime = Mathf.Clamp(currentFadeTime, 0f, fadeDuration);
        float alpha = currentFadeTime / fadeDuration;
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
