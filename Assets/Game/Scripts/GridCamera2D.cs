using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCamera2D : MonoBehaviour
{
    [SerializeField]
    public GameObject follows;
    [SerializeField]
    private BoxCollider2D currentBounds;  // Current bounds
    [SerializeField]
    private float depth = -10;
    [SerializeField]
    [Range(0.1f, 10)]
    private float transitionSpeed;
    [SerializeField]
    private bool instantTransition;    
    [SerializeField]
    [Range(0, 1)]
    private float horizontalThreshold = 0.33f;
    [SerializeField]
    [Range(0, 1)]
    private float verticalThreshold = 0.33f;
    void Start()
    {
        if (follows == null)
        {
            Debug.Log("follows not given on the inspector");
        }
    }
    public void UpdateCurrentBounds(BoxCollider2D newBounds)
    {
        currentBounds = newBounds;
    }

    bool IsWithinCenterRegion(Vector3 position)
    {
        float horizontalLimit = Camera.main.orthographicSize * Camera.main.aspect * horizontalThreshold;
        float verticalLimit = Camera.main.orthographicSize * verticalThreshold;

        return Mathf.Abs(position.x - transform.position.x) < horizontalLimit &&
               Mathf.Abs(position.y - transform.position.y) < verticalLimit;
    }
    bool IsWithinCenterRegionH(Vector3 position)
    {
        float horizontalLimit = Camera.main.orthographicSize * Camera.main.aspect * horizontalThreshold;

        return Mathf.Abs(position.x - transform.position.x) < horizontalLimit;
    }
    bool IsWithinCenterRegionV(Vector3 position)
    {
        float verticalLimit = Camera.main.orthographicSize * verticalThreshold;

        return Mathf.Abs(position.y - transform.position.y) < verticalLimit;
    }

    bool IsOutsideViewport(Vector3 position)
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);
        return viewportPosition.y < 0 || viewportPosition.y > 1;
    }

    Vector3 GetClampedPosition(Vector3 targetPosition)
    {
        float cameraHeight = 2f * Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        float minX = currentBounds.bounds.min.x + cameraWidth / 2;
        float maxX = currentBounds.bounds.max.x - cameraWidth / 2;
        float minY = currentBounds.bounds.min.y + cameraHeight / 2;
        float maxY = currentBounds.bounds.max.y - cameraHeight / 2;

        float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(clampedX, clampedY, targetPosition.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (follows == null || currentBounds == null)
        {
            return;
        }


        Vector3 targetPosition = follows.transform.position;
        targetPosition.z = depth;
        if (IsOutsideViewport(follows.transform.position))
        {
            Vector3 targetPos = transform.position;

            // 객체가 뷰포트 아래쪽에 있는지 확인
            if (follows.transform.position.y < Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y)
            {
                targetPos.y = follows.transform.position.y + Camera.main.orthographicSize;
            }
            // 객체가 뷰포트 위쪽에 있는지 확인
            else if (follows.transform.position.y > Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y)
            {
                targetPos.y = follows.transform.position.y - Camera.main.orthographicSize;
            }

            targetPos.z = depth; // 적절한 깊이 값으로 설정
            transform.position = GetClampedPosition(targetPos); // 새 위치가 원하는 범위 내에 있는지 확인
            return;
        }
        Vector3 newPosition;
        Vector3 tmpPosition;
        newPosition = Vector3.Lerp(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
        tmpPosition = targetPosition;
        targetPosition = transform.position;

        if (!IsWithinCenterRegionV(tmpPosition))
        {
            targetPosition.y = newPosition.y;
        }
        if (!IsWithinCenterRegionH(tmpPosition))
        {
            targetPosition.x = newPosition.x;
        }

        transform.position = GetClampedPosition(targetPosition);
    }
}
