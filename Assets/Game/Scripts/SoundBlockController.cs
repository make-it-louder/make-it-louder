using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SoundBlockController : MonoBehaviourPun
{
    [SerializeField]
    SoundEventManager soundManager;
    SoundSubscriber input;
    Rigidbody2D rb;

    private float maxY;
    private float minY;
    private float curY;

    public float dropPower = 0.1f;

    // limit operating time of soundBlock
    private bool isMovingUp = false;

    private float upTime = 0.0f;
    private float disableInputTime = 0.0f;

    public float maxUpTime = 3.0f;
    public float disavleInputPeriod = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        maxY = transform.position.y + 3;
        minY = transform.position.y;
        input = soundManager.Subscribe(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            Debug.Log("TransferOwnership");
        }
        if (!photonView.IsMine)
        {
            return;
        }
            if (disableInputTime > 0.0f)
        {
            disableInputTime -= Time.deltaTime;
        }
        else
        {
            if (input.normalizedDB > 0.0f)
            {
                isMovingUp = true;
                upTime += Time.deltaTime;
            }
            else
            {
                isMovingUp = false;
            }

            if (upTime >= maxUpTime)
            {
                disableInputTime = disavleInputPeriod;
                upTime = 0.0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (PhotonNetwork.IsMasterClient && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            Debug.Log("TransferOwnership");
        }
        if (photonView.IsMine)
        {
            moveUp();
        }
    }

    void moveUp()
    {

        if (isMovingUp)
        {
            curY = transform.position.y;
            Vector2 newPosition = rb.position + new Vector2(0, 0.1f);
            if (newPosition.y > maxY)
            {
                newPosition.y = maxY;
            }
            rb.MovePosition(newPosition);

        }
        else
        {
            Vector2 newPosition = rb.position + new Vector2(0, -dropPower);
            if (newPosition.y < minY)
            {
                newPosition.y = minY;
            }
            rb.MovePosition(newPosition);
        }
    }
}
