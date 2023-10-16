using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerFollowingCameraBehavior : MonoBehaviour
{
    public GameObject target;
    public Vector2 centerXZ;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = target.transform.position;
        float targetRadius = new Vector2(targetPos.x, targetPos.z).magnitude;
        float radiusRatio = radius / targetRadius;
        transform.position = new Vector3(target.transform.position.x * radiusRatio, target.transform.position.y, target.transform.position.z * radiusRatio);
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
    }
}
