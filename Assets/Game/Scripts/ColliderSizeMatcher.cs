using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ColliderSizeMatcher : MonoBehaviour
{
    [SerializeField]
    private float margin = 0.1f;  // Set a margin value to keep the child collider smaller
    [SerializeField]
    private BoxCollider2D parentCollider;
    [SerializeField]
    private BoxCollider2D childCollider;

    void Start()
    {
        if (childCollider != parentCollider)  // Ensure not to modify the parent's collider
        {
            childCollider.size = new Vector2(
                parentCollider.size.x - margin * 2,  // Subtract the margin from both sides on x-axis
                parentCollider.size.y - margin * 2   // Subtract the margin from both sides on y-axis
            );
            childCollider.offset = parentCollider.offset;
        }
        
    }
    private void Update()
    {
        if (childCollider != parentCollider)  // Ensure not to modify the parent's collider
        {
            childCollider.size = new Vector2(
                parentCollider.size.x - margin * 2,  // Subtract the margin from both sides on x-axis
                parentCollider.size.y - margin * 2   // Subtract the margin from both sides on y-axis
            );
            childCollider.offset = parentCollider.offset;
        }
    }
}
