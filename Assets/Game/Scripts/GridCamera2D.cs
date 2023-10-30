using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCamera2D : MonoBehaviour
{
    [SerializeField]
    public GameObject follows;
    [SerializeField]
    private float depth = -10;
    [SerializeField]
    [Range(1, 10)]
    private float transitionSpeed;
    [SerializeField]
    [Range(0, 1)]
    private float horizontalThreshold = 0.33f;  // Set a threshold for horizontal distance
    [SerializeField]
    [Range(0, 1)]
    private float verticalThreshold = 0.33f;  // Set a threshold for vertical distance

    void Start()
    {
        if (follows == null)
        {
            Debug.Log("follows not given on the inspector");
        }
    }

    bool IsWithinCenterRegion(Vector3 position)
    {
        // Adjusting the size of the center region based on horizontal and vertical thresholds
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

    // Update is called once per frame
    void Update()
    {
        if (follows == null)
        {
            return;
        }

        if (IsWithinCenterRegion(follows.transform.position))
        {
            return;  // Do not move the camera if player is within the center region
        }

        if (IsOutsideViewport(follows.transform.position))
        {
            Vector3 targetPos = transform.position;  // Get the current camera position
            targetPos.y = follows.transform.position.y + Camera.main.orthographicSize;  // Adjust only the Y position so the character is at the bottom of the screen
            targetPos.z = depth;  // Set the target z-coordinate to the desired depth
            transform.position = targetPos;  // Move the camera immediately to the target position
            return;
        }

        Vector3 targetPosition = follows.transform.position;
        targetPosition.z = depth;
        transform.position = Vector3.Lerp(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
    }
}
