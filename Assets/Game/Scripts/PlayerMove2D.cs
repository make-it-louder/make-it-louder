using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System;
using UltimateClean;

public class PlayerMove2D : MonoBehaviourPun
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    new BoxCollider2D collider;
    new PlayerRenderManager renderer;
    private AreaEffector2D effector;

    public MicInputManager micInput;
    private bool isHappy = false; // "Happy" 상태를 추적하는 변수
    private bool isDamage = false; // "Damage" 상태를 감정 표현으로 사용
    private bool isHello = false; // "Hello" 상태를 감정 표현으로 사용

    public float speed;
    public float jumpPower;
    float inputV;
    float inputH;
    public bool isGrounded { get; set; } = false;
    private float jumpTimer;
    public TMP_Text jumpCountText;  //  UI
    public int jumpCount = 0;
    public int isClear = 0;
    public TMP_Text playTimeText;  // UI

    public bool IgnoreInput { get; set; }
    public bool isChatting { get; set; }

    public AudioSource jumpSound; // 점프 효과음을 위한 AudioSource

    private float emotionDuration = 3f; // 감정 상태가 지속되는 시간

    public Popup clearPopup;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        renderer = transform.Find("Renderer").GetComponent<PlayerRenderManager>();
        //rb.centerOfMass = rb.centerOfMass - new Vector2(0, 0.15f);

        segmentLength = (maxY - minY) / 4f; // Skybox

        jumpSound = GetComponent<AudioSource>(); // 점프사운드 정의

        GameObject windEffector = GameObject.FindGameObjectWithTag("windEffector");
        if (windEffector != null)
        {
            effector = windEffector.GetComponent<AreaEffector2D>();
        }
        SetParent();
    }
    public float playTime = 0f;

    public Material[] SkyboxMaterials;

    private float minY = -5.178f;
    private float maxY = 43.000f;
    private float segmentLength;

    // Skybox
    private Material currentSkyboxMaterial;
    private Material targetSkyboxMaterial;
    private float lerpValue = 0f;
    private float lerpSpeed = 0.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonView.IsMine) { return; }

        if (inputH != 0 && !IgnoreInput && !isChatting)
        {
            rb.velocity = new Vector2(inputH * speed, rb.velocity.y);
        }
        //Debug.Log($"inputV > 0 : {inputV > 0}, isGrounded(): {isGrounded()}");
        if (inputV > 0 && !IgnoreInput && !isChatting && (isGroundedAndRay() || isClear == 1) && jumpTimer <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * jumpPower);
            jumpCount++;
            jumpTimer = -jumpPower / (Physics.gravity.y * rb.gravityScale);
            if (isClear == 1)
            {
                jumpTimer = 0.3f; //무한 점프가 돼도 0.3초 딜레이 -> 무한 점프 방지
            }
            if (photonView.IsMine)
            {
                UpdateJumpCountUI();   // UI
            }
            jumpSound.Play();  // 점프 효과음 재생
        }

        if (!isGroundedAndRay() && micInput.DB > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -3.5f));
        }

        if ((rb.velocity.x < -0.003f || rb.velocity.x > 0.003f) && renderer.ViewDirection != (rb.velocity.x > 0.0f))
        {
            renderer.FlipDirection();
            photonView.RPC("SyncSetDirection", RpcTarget.Others, renderer.ViewDirection, renderer.ViewFront);
        }
        float hVelocity = Mathf.Abs(rb.velocity.x);
        float vVelocity = rb.velocity.y;
        bool isGrounded = isGroundedAndRay();
        renderer.SetAnimatorFloat("hVelocity", hVelocity);
        renderer.SetAnimatorFloat("vVelocity", vVelocity);
        renderer.SetAnimatorBool("isGrounded", isGrounded);
        photonView.RPC("SyncSetAnimation", RpcTarget.All, hVelocity, vVelocity, isGrounded);

        // AreaEffector Enable False When isGrounded() == true
        if (effector != null && isGroundedAndRay())
        {
            effector.enabled = false;
        }
        // AreaEffector Enable True When isGrounded() == false
        else if (effector != null && !isGroundedAndRay())
        {
            effector.enabled = true;
        }
        jumpTimer -= Time.fixedDeltaTime;

    }
    void Update()
    {
        if (!photonView.IsMine || IgnoreInput || isChatting)
        {
            return;
        }
        inputV = Input.GetAxis("Jump");
        inputH = Input.GetAxis("Horizontal");

        if (inputH != 0)
        {
            isHappy = false;
            isDamage = false;
            isHello = false;
            renderer.SetAnimatorBool("isHappy", false);
            renderer.SetAnimatorBool("isDamage", false);
            renderer.SetAnimatorBool("isHello", false);
            renderer.ViewFront = false;
        }
        playTime += Time.deltaTime;
        UpdatePlayTimeUI();

        ChangeSkyboxMaterial();

        // Skybox
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
            renderer.ViewFront = isHappy;
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
            renderer.ViewFront = false;
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
            renderer.ViewFront = isHello;
        }
        // 방향키 또는 점프키를 눌렀을 때 isHappy를 false로 설정
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Jump"))
        {
            renderer.ViewFront = false;
        }
        photonView.RPC("SyncSetDirection", RpcTarget.Others, renderer.ViewDirection, renderer.ViewFront);
        photonView.RPC("SyncSetEmotion", RpcTarget.Others, isHappy, isDamage, isHello);
    }

    void UpdateJumpCountUI()
    {
        if (jumpCountText != null)
        {
            jumpCountText.text = "JumpCount: " + jumpCount;
        }
    }

    void UpdatePlayTimeUI()
    {
        int minutes = (int)(playTime / 60);
        int seconds = (int)(playTime % 60);
        if (playTimeText != null)
        {
            playTimeText.text = $"PlayTime: {minutes:00}:{seconds:00}";
        }
    }



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

    /*    bool isGrounded()
        {
            Vector3 feet = rb.transform.position + transform.up * transform.lossyScale.y * (collider.offset.y - collider.size.y / 2 - 0.01f) + transform.right * transform.lossyScale.x * collider.offset.x; ;
            Vector3 feetLeft = feet - transform.right * transform.lossyScale.x * (collider.size.x / 2) * 0.97f;
            Vector3 feetRight = feet + transform.right * transform.lossyScale.x * (collider.size.x / 2) * 0.97f;
            Debug.DrawLine(feetLeft, feetRight, Color.red);

            return (Physics2D.OverlapPoint(feetLeft,1<<LayerMask.NameToLayer("Ground")) != null) || (Physics2D.OverlapPoint(feetRight,1<<LayerMask.NameToLayer("Ground")) != null || (Physics2D.OverlapPoint(feetRight, 1 << LayerMask.NameToLayer("Player"))) || (Physics2D.OverlapPoint(feetLeft, 1 << LayerMask.NameToLayer("Player"))));
        }*/
    bool isGroundedAndRay()
    {
        // 발의 중앙 위치를 계산합니다.
        Vector3 feetPosition = rb.transform.position + transform.up * transform.lossyScale.y * (collider.offset.y - collider.size.y / 2 - 0.01f);

        // 레이캐스트의 길이를 조절합니다. 너무 길면 플랫폼에 도달하기 전에 그라운드로 인식될 수 있습니다.
        float rayLength = 0.1f;

        // 발의 왼쪽 끝과 오른쪽 끝을 계산합니다.
        Vector3 feetLeft = feetPosition - transform.right * transform.lossyScale.x * collider.size.x / 2;
        Vector3 feetRight = feetPosition + transform.right * transform.lossyScale.x * collider.size.x / 2;

        // 왼쪽과 오른쪽 끝에서 바닥을 감지하기 위한 레이캐스트를 발사합니다.
        RaycastHit2D hitLeft = Physics2D.Raycast(feetLeft, -transform.up, rayLength, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(feetRight, -transform.up, rayLength, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D hitPlayerleft = Physics2D.Raycast(feetRight, -transform.up, rayLength, 1 << LayerMask.NameToLayer("Player"));
        RaycastHit2D hitPlayerRight = Physics2D.Raycast(feetRight, -transform.up, rayLength, 1 << LayerMask.NameToLayer("Player"));


        // 레이캐스트를 시각화합니다 (디버깅 용도).
        Debug.DrawRay(feetLeft, -transform.up * rayLength, Color.red);
        Debug.DrawRay(feetRight, -transform.up * rayLength, Color.red);

        // 레이캐스트가 왼쪽 끝이나 오른쪽 끝 중 어느 한 곳에서라도 무언가에 부딪혔는지 확인합니다.
        return (hitLeft.collider != null || hitRight.collider != null || hitPlayerleft.collider != null || hitPlayerRight.collider != null);

    }


    IEnumerator ResetEmotionAfterDelay(string emotion)
    {
        yield return new WaitForSeconds(emotionDuration);

        switch (emotion)
        {
            case "isHappy":
                isHappy = false;
                break;
            case "isDamage":
                isDamage = false;
                break;
            case "isHello":
                isHello = false;
                break;
        }
        renderer.SetAnimatorBool("isHappy", false);
        renderer.SetAnimatorBool("isDamage",false);
        renderer.SetAnimatorBool("isHello", false);
        photonView.RPC("SyncSetEmotion", RpcTarget.Others, isHappy, isDamage, isHello);
        renderer.ViewFront = false;
        photonView.RPC("SyncSetDirection", RpcTarget.Others, renderer.ViewDirection, renderer.ViewFront);
    }
    [PunRPC]
    public void SyncSetDirection(bool ViewDirection, bool ViewFront, PhotonMessageInfo info)
    {
        PlayerMove2D move = info.photonView.GetComponent<PlayerMove2D>();
        PlayerRenderManager renderer = move.renderer;
        if (renderer == null)
        {
            return;
        }
        renderer.ViewDirection = ViewDirection;
        renderer.ViewFront = ViewFront;
    }
    [PunRPC]
    public void SyncSetEmotion(bool isHappy, bool isDamage, bool isHello, PhotonMessageInfo info)
    {
        PlayerMove2D move = info.photonView.GetComponent<PlayerMove2D>();
        PlayerRenderManager renderer = move.renderer;
        if (renderer == null)
        {
            return;
        }
        renderer.SetAnimatorBool("isHappy", isHappy);
        renderer.SetAnimatorBool("isDamage", isDamage);
        renderer.SetAnimatorBool("isHello", isHello);
    }
    [PunRPC]
    public void SyncSetAnimation(float hVelocity, float vVelocity, bool isGrounded, PhotonMessageInfo info)
    {
        PlayerMove2D move = info.photonView.GetComponent<PlayerMove2D>();
        PlayerRenderManager renderer = move?.renderer;
        if (renderer == null)
        {
            return;
        }
        try
        {
            renderer.SetAnimatorFloat("hVelocity", hVelocity);
            renderer.SetAnimatorFloat("vVelocity", vVelocity);
            renderer.SetAnimatorBool("isGrounded", isGrounded);
        }
        catch (NullReferenceException e)
        {
            Debug.LogError("NullReferenceException");
            Debug.LogError($"renderer:{renderer}");
            Debug.LogError($"animatorController: {renderer.animatorController}");
            Debug.LogError($"exception: {e}");
        }
    }

    //골인
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("ClearPoint"))
        {
            OnPlayerEnter();
        }
    }

    async void OnPlayerEnter()
    {
        if (isClear == 1) return;
        else {
            if (!photonView.IsMine)
            {
                return;
            }
            isClear = 1;
            Debug.Log("골인");
            Popup copy = Instantiate(clearPopup, clearPopup.transform.parent);
            copy.gameObject.SetActive(true);
            copy.Open();
            string userId = RecordManager.Instance.currentId;
            await RecordManager.Instance.UpdateClearRecords("map1", jumpCount, playTime); // 최소점프, 최소 클리어타임 업데이트
            await RecordManager.Instance.UpdateEndGameData("map1", playTime, jumpCount, 0); // 이 방에서의 기록 중간업데이트
            await RankingManager.Instance.UpdateClearTimeRank(playTime, userId); // 클리어했으니 클리어타임 랭킹 업데이트
            await RankingManager.Instance.UpdateMinJumpRank(jumpCount, userId); // 클리어했으니 점프랭킹 업데이트
        }
    }

    public void SetParent()
    {
        photonView.RPC("SyncSetParent", RpcTarget.AllBuffered);
    }
    [PunRPC]
    void SyncSetParent(PhotonMessageInfo info)
    {
        GameObject parent = GameObject.Find("CharacterList");
        if (parent != null) {
            info.photonView.gameObject.transform.parent = parent.transform;
        }
        else
        {
            Debug.LogError("Cannot find parent");
        }
    }
}