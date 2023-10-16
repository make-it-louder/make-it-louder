using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCamera2D : MonoBehaviour
{
    [SerializeField]
    private GameObject follows;
    [SerializeField]
    private Vector2 gridScale;
    [SerializeField]
    private float depth = -10;
    void Start()
    {
        if (follows == null)
        {
            Debug.LogError("follows not given on the inspector");
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
        Vector2Int targetGridIdx = Pos2GridIdx(follows.transform.position);
        Vector3 targetPos = GridIdx2Pos(targetGridIdx);
        transform.position = Vector3.Lerp(transform.position, targetPos, 3 * Time.deltaTime);
    }
}
