using UnityEngine;

public class BoundTrigger : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D parentCollider;
    private void OnTriggerEnter2D(Collider2D other)
    {
        GridCamera2D cameraScript = Camera.main.GetComponent<GridCamera2D>();
        if (cameraScript != null && other.gameObject == cameraScript.follows)
        {
            cameraScript.UpdateCurrentBounds(parentCollider);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Optionally, handle when the character exits the bounds
    }
}
