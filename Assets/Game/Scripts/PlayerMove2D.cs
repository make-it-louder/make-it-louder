using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerMove2D : MonoBehaviourPun
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    new BoxCollider2D collider;
    new PlayerRenderManager renderer;

    public MicInputManager micInput;
    private bool isHappy = false; // "Happy" 상태를 추적하는 변수
    private bool isDamage = false; // "Damage" 상태를 감정 표현으로 사용
    private bool isHello = false; // "Hello" 상태를 감정 표현으로 사용
    public float speed;
    public float jumpPower;
    float inputV;
    float inputH;

    public TMP_Text jumpCountText;  // ���� Ƚ���� ǥ���� UI �ؽ�Ʈ
    public int jumpCount = 0;  // ������ Ƚ��

    public TMP_Text playTimeText;  // �÷��� Ÿ���� ǥ���ϴ� UI

    public bool IgnoreInput { get; set; }
    public bool isChatting { get; set; }

    public AudioSource jumpSound; // 점프 효과음을 위한 AudioSource

    private float emotionDuration = 3f; // 감정 상태가 지속되는 시간

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        renderer = transform.Find("Renderer").GetComponent<PlayerRenderManager>();
        //rb.centerOfMass = rb.centerOfMass - new Vector2(0, 0.15f);

        segmentLength = (maxY - minY) / 4f; // Skybox ���ϴ� ���� ����

        jumpSound = GetComponent<AudioSource>(); // 점프사운드 정의
    }
    private float playTime = 0f; // �÷��� Ÿ��

    // ���ȭ�� ���ø���
    public Material[] SkyboxMaterials;

    // y���� �ִ�,�ּҳ��� ����
    private float minY = -5.178f;
    private float maxY = 43.000f;
    private float segmentLength;

    // �ε巯�� Skybox ��ȯ�� ���� ������
    private Material currentSkyboxMaterial;
    private Material targetSkyboxMaterial;
    private float lerpValue = 0f;
    private float lerpSpeed = 0.5f;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (inputH != 0 && !IgnoreInput && !isChatting)
        {
            rb.velocity = new Vector2(inputH * speed, rb.velocity.y);
        }
        //Debug.Log($"inputV > 0 : {inputV > 0}, isGrounded(): {isGrounded()}");
        if (inputV > 0 && isGrounded() && !IgnoreInput && !isChatting)
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
        }

        if (!isGrounded() && micInput.DB > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -3.5f));
        }

        if ((rb.velocity.x < -0.003f || rb.velocity.x > 0.003f) && renderer.ViewDirection != (rb.velocity.x > 0.0f))
        {
            renderer.FlipDirection();
        }
        renderer.SetAnimatorFloat("hVelocity", Mathf.Abs(rb.velocity.x));
        renderer.SetAnimatorFloat("vVelocity", rb.velocity.y);
        renderer.SetAnimatorBool("isGrounded", isGrounded());

        // ���� UI ������Ʈ
        if (inputV > 0 && isGrounded() && !IgnoreInput && !isChatting)
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
            jumpCount++;           // ������ ������ ī��Ʈ ����
            UpdateJumpCountUI();   // UI ������Ʈ
            jumpSound.Play();  // 점프 효과음 재생
        }

    }
    void Update()
    {
        if (photonView.IsMine && !IgnoreInput && !isChatting)
        {
            inputV = Input.GetAxis("Jump");
            inputH = Input.GetAxis("Horizontal");
            // �÷��� Ÿ�� ����
            playTime += Time.deltaTime;
            UpdatePlayTimeUI();

            // Skybox Material ����
            ChangeSkyboxMaterial();

            // �ε巯�� Skybox ��ȯ
            SmoothSkyboxTransition();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isHappy = !isHappy;
                renderer.SetAnimatorBool("isHappy", isHappy);

                if (isHappy)
                {
                    isDamage = false;
                    renderer.SetAnimatorBool("isDamage", isDamage);
                    isHello = false;
                    renderer.SetAnimatorBool("isHello", isHello);

                    StopAllCoroutines(); // 다른 감정 상태의 코루틴 중지
                    StartCoroutine(ResetEmotionAfterDelay("isHappy"));
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                isDamage = !isDamage;
                renderer.SetAnimatorBool("isDamage", isDamage);

                if (isDamage)
                {
                    isHappy = false;
                    renderer.SetAnimatorBool("isHappy", isHappy);
                    isHello = false;
                    renderer.SetAnimatorBool("isHello", isHello);

                    StopAllCoroutines(); // 다른 감정 상태의 코루틴 중지
                    StartCoroutine(ResetEmotionAfterDelay("isDamage"));
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                isHello = !isHello;
                renderer.SetAnimatorBool("isHello", isHello);

                if (isHello)
                {
                    isHappy = false;
                    renderer.SetAnimatorBool("isHappy", isHappy);
                    isDamage = false;
                    renderer.SetAnimatorBool("isDamage", isDamage);

                    StopAllCoroutines(); // 다른 감정 상태의 코루틴 중지
                    StartCoroutine(ResetEmotionAfterDelay("isHello"));
                }
            }

            // 방향키 또는 점프키를 눌렀을 때 isHappy를 false로 설정
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
            {
                isHappy = false;
                renderer.SetAnimatorBool("isHappy", isHappy);

                isDamage = false;
                renderer.SetAnimatorBool("isDamage", isDamage);

                isHello = false;
                renderer.SetAnimatorBool("isHello", isHello);
            }
        }
    }

    // ����Ƚ�� ī��Ʈ
    void UpdateJumpCountUI()
    {
        if (jumpCountText != null)
        {
            jumpCountText.text = "JumpCount: " + jumpCount;
        }
        else
        {
            Debug.Log("Cannot find jumpCountText");
        }
    }
    // �÷���Ÿ�� ��ȭ
    void UpdatePlayTimeUI()
    {
        int minutes = (int)(playTime / 60);
        int seconds = (int)(playTime % 60);
        if (playTimeText != null)
        {
            playTimeText.text = $"PlayTime: {minutes:00}:{seconds:00}";
        }
        else
        {
            Debug.Log("Cannot find jumpCountText");
        }
    }


    // ���ȭ�� ��ȭ
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

    // �ε巯�� ���ȭ�� ��ȭ
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

        return (Physics2D.OverlapPoint(feetLeft,1<<LayerMask.NameToLayer("Ground")) != null) || (Physics2D.OverlapPoint(feetRight,1<<LayerMask.NameToLayer("Ground")) != null || (Physics2D.OverlapPoint(feetRight, 1 << LayerMask.NameToLayer("Player"))) || (Physics2D.OverlapPoint(feetLeft, 1 << LayerMask.NameToLayer("Player"))));
    }

    IEnumerator ResetEmotionAfterDelay(string emotion)
    {
        yield return new WaitForSeconds(emotionDuration);

        switch (emotion)
        {
            case "isHappy":
                isHappy = false;
                renderer.SetAnimatorBool("isHappy", isHappy);
                break;
            case "isDamage":
                isDamage = false;
                renderer.SetAnimatorBool("isDamage", isDamage);
                break;
            case "isHello":
                isHello = false;
                renderer.SetAnimatorBool("isHello", isHello);
                break;
        }
    }
}