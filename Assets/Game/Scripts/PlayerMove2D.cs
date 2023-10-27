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
    new SpriteRenderer renderer;
    Animator animator;

    public MicInputManager micInput;

    public float speed;
    public float jumpPower;
    float inputV;
    float inputH;

    public TMP_Text jumpCountText;  // ���� Ƚ���� ǥ���� UI �ؽ�Ʈ
    public int jumpCount = 0;  // ������ Ƚ��

    public TMP_Text playTimeText;  // �÷��� Ÿ���� ǥ���ϴ� UI

    public bool IgnoreInput { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        renderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        animator = transform.Find("Renderer").GetComponent<Animator>();
        //rb.centerOfMass = rb.centerOfMass - new Vector2(0, 0.15f);

        segmentLength = (maxY - minY) / 4f; // Skybox ���ϴ� ���� ����
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
        if (inputH != 0 && !IgnoreInput)
        {
            rb.velocity = new Vector2(inputH * speed, rb.velocity.y);
        }
        //Debug.Log($"inputV > 0 : {inputV > 0}, isGrounded(): {isGrounded()}");
        if (inputV > 0 && isGrounded() && !IgnoreInput)
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
        }

        if (!isGrounded() && micInput.DB > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -3.5f));
        }

        if ((rb.velocity.x < -0.003f) != renderer.flipX)
        {
            renderer.flipX = rb.velocity.x < 0;
        }
        animator.SetFloat("hVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("vVelocity", rb.velocity.y);
        animator.SetBool("isGrounded", isGrounded());

        // ���� UI ������Ʈ
        if (inputV > 0 && isGrounded() && !IgnoreInput)
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
            jumpCount++;           // ������ ������ ī��Ʈ ����
            UpdateJumpCountUI();   // UI ������Ʈ
        }

    }
    void Update()
    {
        if (photonView.IsMine && !IgnoreInput)
        {
            inputV = Input.GetAxis("Jump");
            inputH = Input.GetAxis("Horizontal");
        }
        // �÷��� Ÿ�� ����
        playTime += Time.deltaTime;
        UpdatePlayTimeUI();

        // Skybox Material ����
        ChangeSkyboxMaterial();

        // �ε巯�� Skybox ��ȯ
        SmoothSkyboxTransition();
    }
    // ����Ƚ�� ī��Ʈ
    void UpdateJumpCountUI()
    {
        if (jumpCountText != null)
        {
            jumpCountText.text = "JumpCount: " + jumpCount;
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
}