using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMove2D : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    new BoxCollider2D collider;
    new SpriteRenderer renderer;
    Animator animator;

    public MicInputManager micInput;

    public float speed;
    public float jumpPower;
    float inputV;
    float inputH;

    public TMP_Text jumpCountText;  // 점프 횟수를 표시할 UI 텍스트
    public int jumpCount = 0;  // 점프한 횟수

    public TMP_Text playTimeText;  // 플레이 타임을 표시하는 UI
    private float playTime = 0f; // 플레이 타임

    // 배경화면 템플릿들
    public Material[] SkyboxMaterials;

    // y축의 최대,최소높이 설정
    private float minY = -5.178f;
    private float maxY = 43.000f;
    private float segmentLength;

    // 부드러운 Skybox 전환을 위한 변수들
    private Material currentSkyboxMaterial;
    private Material targetSkyboxMaterial;
    private float lerpValue = 0f;
    private float lerpSpeed = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        renderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        animator = transform.Find("Renderer").GetComponent<Animator>();
        //rb.centerOfMass = rb.centerOfMass - new Vector2(0, 0.15f);

        segmentLength = (maxY - minY) / 4f; // Skybox 변하는 기준 설정
    }
    void Update()
    {
        inputV = Input.GetAxis("Jump");
        inputH = Input.GetAxis("Horizontal");

        // 플레이 타임 로직
        playTime += Time.deltaTime;
        UpdatePlayTimeUI();

        // Skybox Material 변경
        ChangeSkyboxMaterial();

        // 부드러운 Skybox 전환
        SmoothSkyboxTransition();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputH != 0)
        {
            rb.velocity = new Vector2(inputH * speed, rb.velocity.y);
        }
        if ((inputH != 0) && (inputH < 0) != renderer.flipX)
        {
            renderer.flipX = inputH < 0;
        }
        //Debug.Log($"inputV > 0 : {inputV > 0}, isGrounded(): {isGrounded()}");
        if (inputV > 0 && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
        }

        if (!isGrounded() && micInput.DB > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -3.5f));
        }
        animator.SetFloat("hVelocity", Mathf.Abs(inputH));
        animator.SetFloat("vVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded());

        // 점프 UI 업데이트
        if (inputV > 0 && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
            jumpCount++;           // 점프할 때마다 카운트 증가
            UpdateJumpCountUI();   // UI 업데이트
        }

    }
    // 점프횟수 카운트
    void UpdateJumpCountUI()
    {
        jumpCountText.text = "JumpCount: " + jumpCount;
    }
    // 플레이타임 변화
    void UpdatePlayTimeUI()
    {
        int minutes = (int)(playTime / 60);
        int seconds = (int)(playTime % 60);
        playTimeText.text = $"PlayTime: {minutes:00}:{seconds:00}";
    }


    // 배경화면 변화
    void ChangeSkyboxMaterial()
    {
        float currentY = transform.position.y;
        Material newTarget = null;

        if (currentY < minY + segmentLength)
        {
            newTarget = SkyboxMaterials[0];
        }
        else if (currentY < minY + 2 * segmentLength)
        {
            newTarget = SkyboxMaterials[1];
        }
        else if (currentY < minY + 3 * segmentLength)
        {
            newTarget = SkyboxMaterials[2];
        }
        else
        {
            newTarget = SkyboxMaterials[3];
        }

        if (newTarget != targetSkyboxMaterial)
        {
            currentSkyboxMaterial = RenderSettings.skybox;
            targetSkyboxMaterial = newTarget;
            lerpValue = 0f;
        }
    }

    // 부드러운 배경화면 변화
    void SmoothSkyboxTransition()
    {
        if (lerpValue < 1f && targetSkyboxMaterial != null)
        {
            lerpValue += Time.deltaTime * lerpSpeed;
            Material lerpMat = new Material(currentSkyboxMaterial);
            lerpMat.Lerp(currentSkyboxMaterial, targetSkyboxMaterial, lerpValue);
            RenderSettings.skybox = lerpMat;
            DynamicGI.UpdateEnvironment();
        }
    }

    bool isGrounded()
    {
        Vector3 feet = rb.transform.position + transform.up * transform.lossyScale.y * (collider.offset.y - collider.size.y / 2 - 0.01f) + transform.right * transform.lossyScale.x * collider.offset.x; ;
        Vector3 feetLeft = feet - transform.right * transform.lossyScale.x * (collider.size.x / 2) * 0.97f;
        Vector3 feetRight = feet + transform.right * transform.lossyScale.x * (collider.size.x / 2) * 0.97f;
        Debug.DrawLine(feetLeft, feetRight, Color.red);
        return (Physics2D.OverlapPoint(feetLeft) != null) || (Physics2D.OverlapPoint(feetRight) != null);
    }
}