using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCamera2D : MonoBehaviour
{
    [SerializeField]
    public GameObject follows;
    [SerializeField]
    private Vector2 gridScale;
    [SerializeField]
    private float depth = -10;
    [SerializeField]
    [Range(1, 10)]
    private float transitionSpeed;
    [SerializeField]
    private bool instantTransition;    
    void Start()
    {
        if (follows == null)
        {
            Debug.Log("follows not given on the inspector");
        }
    }

    Vector3 GridIdx2Pos(Vector2Int gridIdx)
    {
        Vector3 result;
        result.y = gridIdx.y * gridScale.y;
        result.x = gridIdx.x * gridScale.x;
        result.z = depth;
        return result;
    }
    Vector2Int Pos2GridIdx(Vector3 pos)
    {
        Vector2Int gridIdx = new Vector2Int();
        gridIdx.x = (int)Mathf.Round(pos.x / gridScale.x);
        gridIdx.y = (int)Mathf.Round(pos.y / gridScale.y);
        return gridIdx;
    }
    // Update is called once per frame
    void Update()
    {
        if (follows == null)
        {
            return;
        }
        Vector2Int targetGridIdx = Pos2GridIdx(follows.transform.position);
        Vector3 targetPos = GridIdx2Pos(targetGridIdx);
        if (instantTransition)
        {
            transform.position = targetPos;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, transitionSpeed * Time.deltaTime);
        }
    }
}
