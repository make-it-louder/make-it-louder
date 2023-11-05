using UnityEngine;

public class ParallaxScrolling : MonoBehaviour
{
    public Transform[] backgrounds; // 배경 레이어를 위한 배열
    private float[] parallaxScales; // 움직임의 비율
    public float smoothing = 1f;    // 부드러운 움직임을 위한 변수

    public float verticalParallaxScale = 2f; // 세로 움직임의 비율을 조정하는 변수

    private Transform cam;          // 메인 카메라의 참조
    private Vector3 previousCamPos; // 이전 프레임에서 카메라의 위치

    // 최초에 호출되는 함수
    void Awake()
    {
        // 카메라 참조 설정
        cam = Camera.main.transform;
    }

    // 시작 시 호출되는 함수
    void Start()
    {
        // 이전 카메라 위치를 초기화
        previousCamPos = cam.position;

        // 스케일링 비율을 초기화
        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // 프레임마다 호출되는 함수
    void Update()
    {
        // 각 배경에 대해 반복
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // 가로 파럴랙스는 반대 방향으로 카메라의 움직임에 비례하여 움직임
            float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScales[i] * verticalParallaxScale; // 세로 파럴랙스 추가

            // 대상 x, y 위치를 설정
            float backgroundTargetPosX = backgrounds[i].position.x + parallaxX;
            float backgroundTargetPosY = backgrounds[i].position.y + parallaxY;

            // 대상 위치를 생성
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

            // 부드러운 움직임을 위한 보간
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // 이전 카메라 위치 업데이트
        previousCamPos = cam.position;
    }
}
