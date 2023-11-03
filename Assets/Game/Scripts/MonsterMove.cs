using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float minX;
    public float maxX;
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

    void ChooseRandomTarget()
    {
        float randomX = Random.Range(minX, maxX);
        targetPosition = new Vector3(randomX, transform.position.y, transform.position.z);
        if (randomX-transform.position.x>0 && !renderer.ViewDirection) {
            renderer.FlipDirection();
        }
        else if (randomX - transform.position.x < 0 && renderer.ViewDirection)
        {
            renderer.FlipDirection();
        }
        waitTime = Random.Range(minWaitTime, maxWaitTime);  // Choose a random wait time
    }


    void ResetWaitTimer()
    {
        waitTimer = 0f;
    }

    void MoveTowardsTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
    void OnDrawGizmos()
    {
        Vector3 startPos = new Vector3(minX, transform.position.y, transform.position.z);
        Vector3 endPos = new Vector3(maxX, transform.position.y, transform.position.z);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, endPos);
    }
}
