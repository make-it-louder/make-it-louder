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

    public float maxXRange;
    public float maxYRange;

    private float initX;
    private float limitX;
    private float maxY;
    private float minY;

    public float dropPower = 0.1f;


    public bool directionLeft = false;
    public bool directionRight = false;

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
        maxY = transform.position.y + maxYRange;
        minY = transform.position.y;
        limitX = transform.position.x + maxXRange;
        initX = transform.position.x;
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
        if (!photonView.IsMine)
        {
            return;
        }
        moveUp();
    }

    void moveUp()
    {


        float xMove = 0f;

        if (directionLeft == true)
        {
            xMove = -0.2f;
        }
        else if (directionRight == true)
        {
            xMove = 0.2f;
        }

        if (isMovingUp)
        {

            Vector2 newPosition = rb.position + new Vector2(xMove, 0.1f);
            if (newPosition.y > maxY)
            {
                newPosition.y = maxY;
            }
            if (newPosition.x > limitX && limitX > initX)
            {
                newPosition.x = limitX;
            }
            else if (newPosition.x < limitX && limitX < initX)
            {
                newPosition.x = limitX;
            }
            rb.MovePosition(newPosition);

        }
        else
        {
            Vector2 newPosition = rb.position + new Vector2(-xMove, -dropPower);
            if (newPosition.y < minY)
            {
                newPosition.y = minY;
            }
            if (newPosition.x < initX && limitX > initX)
            {
                newPosition.x = initX;
            } else if (newPosition.x > initX && limitX < initX)  {
                newPosition.x = initX;
            }
            rb.MovePosition(newPosition);
        }
    }
}
