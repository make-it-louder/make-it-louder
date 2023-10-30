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
    [Range(1, 10)]
    private float transitionSpeed;
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
    void Update()
    {
        if (follows == null || currentBounds == null)
        {
            return;
        }

        if (IsWithinCenterRegion(follows.transform.position))
        {
            return;
        }

        Vector3 targetPosition = follows.transform.position;
        targetPosition.z = depth;

        if (IsOutsideViewport(follows.transform.position))
        {
            Vector3 targetPos = transform.position;
            targetPos.y = follows.transform.position.y + Camera.main.orthographicSize;
            targetPos.z = depth;
            transform.position = GetClampedPosition(targetPos);
            return;
        }

        targetPosition = Vector3.Lerp(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
        transform.position = GetClampedPosition(targetPosition);
    }
}