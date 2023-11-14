using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using System;
using UltimateClean;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMove2D : MonoBehaviourPun
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    new BoxCollider2D collider;
    new PlayerRenderManager renderer;
    private AreaEffector2D effector;
    public LayerMask groundLayer; // "Ground" 레이어
    public LayerMask playerLayer; // "Player"와 "Ground" 레이어
    public Transform bottomBox; // 아래쪽 감지용 자식 오브젝트
    public Transform topBox; // 위쪽 감지용 자식 오브젝트
    public Vector2 detectionBoxSize = new Vector2(1f, 0.2f); // 감지 영역의 크기
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
    public AcheivementManager acheivementManager;

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

        if (!isGroundedAndRay() && micInput.normalizedDB > 0.5)
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


    bool isGroundedAndRay()
    {
        if (DetectGroundBelow())
        {
            if (!DetectAboveHead())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (DetectPlayerBelow())
        {
            return true;
        }
        else { return false; }
    }

    bool DetectGroundBelow()
    {
        Collider2D hit = Physics2D.OverlapBox(bottomBox.position, detectionBoxSize, 0f, groundLayer);
        return hit != null;
    }

    bool DetectPlayerBelow()
    {
        Collider2D hit = Physics2D.OverlapBox(bottomBox.position, detectionBoxSize, 0f, playerLayer);
        return hit != null;
    }

    bool DetectAboveHead()
    {
        Collider2D hit = Physics2D.OverlapBox(topBox.position, detectionBoxSize, 0f, groundLayer);
        return hit != null;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bottomBox.position, detectionBoxSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(topBox.position, detectionBoxSize);
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
            int done = 0;

            Debug.Log("골인");
            Popup copy = Instantiate(clearPopup, clearPopup.transform.parent);
            string userId = RecordManager.Instance.currentId;
            await RecordManager.Instance.UpdateClearRecords("map1", jumpCount, playTime); // 최소점프, 최소 클리어타임 업데이트
            await RecordManager.Instance.UpdateEndGameData("map1", playTime, jumpCount, 0); // 이 방에서의 기록 중간업데이트
            await RankingManager.Instance.UpdateClearTimeRank(playTime, userId); // 클리어했으니 클리어타임 랭킹 업데이트
            await RankingManager.Instance.UpdateMinJumpRank(jumpCount, userId); // 클리어했으니 점프랭킹 업데이트
            done++;
            if (done == 1) {
                string convertedPT = ConvertToTime(playTime);
                copy.gameObject.SetActive(true);
                copy.time.text = string.Format("시간: " + convertedPT);
                copy.jump.text = string.Format("점프: {0:N0} 회", jumpCount);
                copy.done.text = "기록 업데이트 완료!";
                copy.Open();
            }
            else
            {
                string convertedPT = ConvertToTime(playTime);
                copy.gameObject.SetActive(true);
                copy.time.text = string.Format("시간: " + convertedPT);
                copy.jump.text = string.Format("{0:N0} 회", jumpCount);

                copy.done.text = "기록 업데이트에 실패(오류)";
                copy.Open();
            }
            if (acheivementManager != null)
            {
                Debug.Log("acvm 있어요");
                await acheivementManager.UpdateAllAcheivement();
            }
            else
            {
                Debug.Log("acvm이 없어요");
            }
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
            transform.parent = parent.transform;
        }
        else
        {
            Debug.LogError("Cannot find parent");
        }
    }
    private string ConvertToTime(float time)
    {
        // 정수 초 부분과 밀리초 부분 분리
        int intPart = (int)time; // 정수 부분
        int milliPart = (int)((time - intPart) * 1000); // 밀리세컨드 부분

        // TimeSpan 객체로 변환
        TimeSpan toTime = TimeSpan.FromSeconds(intPart);

        // 시간 형식 문자열로 변환
        string timeFormat = string.Format("{0:D2}:{1:D2}:{2:D2}.<size=70%>{3:D2}</size>",
            (int)toTime.TotalHours, // 시간
            toTime.Minutes,         // 분
            toTime.Seconds,         // 초
            (int)milliPart / 10);           // 밀리세컨드

        return timeFormat;
    }
}