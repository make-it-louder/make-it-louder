using UnityEngine;
using Photon.Pun;

public class MonsterMovement : MonoBehaviourPun
{
    private float minX;
    private float maxX;
    public float RangeL = 2f;
    public float RangeR = 2f;
    public float speed = 2f;
    public float minWaitTime = 1f;  // Set the minimum wait time in the inspector
    public float maxWaitTime = 3f;  // Set the maximum wait time in the inspector
    new PlayerRenderManager renderer;
    private Vector3 targetPosition;
    private bool movingToMax;
    private float waitTime;
    private float waitTimer;

    void Start()
    {
        renderer = transform.Find("Renderer").GetComponent<PlayerRenderManager>();
        minX = transform.position.x - RangeL;
        maxX = transform.position.x + RangeR;
        ChooseRandomTarget();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            renderer.SetAnimatorBool("isRun", false);

            if (waitTimer >= waitTime)
            {
                ChooseRandomTarget();
                ResetWaitTimer();
            }
            else
            {
                waitTimer += Time.deltaTime;
            }
        }
        else
        {
            MoveTowardsTarget();
            renderer.SetAnimatorBool("isRun", true);
        }
    }

    [PunRPC]
    public void FlipDirection()
    {
        renderer.FlipDirection();
    }

    void ChooseRandomTarget()
    {
        float randomX = Random.Range(minX, maxX);
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
        if (targetPosition.x-transform.position.x>0 && !renderer.ViewDirection) {
            renderer.FlipDirection();
            photonView.RPC("FlipDirection", RpcTarget.Others);
        }
        else if (targetPosition.x - transform.position.x < 0 && renderer.ViewDirection)
        {
            renderer.FlipDirection();
            photonView.RPC("FlipDirection", RpcTarget.Others);
        }
        waitTime = Random.Range(minWaitTime, maxWaitTime);  // Choose a random wait time
    }


    void ResetWaitTimer()
    {
        waitTimer = 0f;
    }

    void MoveTowardsTarget()
    {
        if (photonView.IsMine)
        {
            Vector3 displacement = targetPosition - transform.position;
            if (displacement.magnitude <= speed * Time.deltaTime)
            {
                transform.position = targetPosition;
            }
            else
            {
                Vector3 direction = displacement.normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }
    void OnDrawGizmos()
    {
        Vector3 startPos = new Vector3(transform.position.x - RangeL, transform.position.y, transform.position.z);
        Vector3 endPos = new Vector3(transform.position.x + RangeR, transform.position.y, transform.position.z);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPos);
    }
}
