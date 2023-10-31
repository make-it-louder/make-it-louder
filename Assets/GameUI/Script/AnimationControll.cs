using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 screenBounds;

    private void Start()
    {
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    private void Update()
    {
        if (transform.position.y < -screenBounds.y)
        {
            Destroy(gameObject);
        }
    }
}
