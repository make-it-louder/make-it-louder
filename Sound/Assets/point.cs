using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class point : MonoBehaviour
{
    // Start is called before the first frame update
    public float minTime = 0;
    public float maxTime = 20.0f;
    public float minDB = -160.0f;
    public float maxDB = 100.0f;
    public float minPitch = 0.0f;
    public float maxPitch = 1000.0f;

    public Canvas canvas;
    public GameObject dbDataPoint;
    public GameObject pitchDataPoint;
    public LinkedList<GameObject> created;
    private float time = 0;
    RectTransform canvasRectTransform;
    float width, height, canvasLeft, canvasTop, canvasRight, canvasBottom;
    void Start()
    {
        created = new LinkedList<GameObject>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        width = canvasRectTransform.rect.width;
        height = canvasRectTransform.rect.height;
        canvasLeft = 10;
        canvasTop = 10;
        canvasRight = width - canvasLeft;
        canvasBottom = height - canvasTop;
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;
        time += deltaTime;
        if (time > maxTime)
        {
            foreach (GameObject go in created)
            {
                Vector3 origPosition = go.transform.position;
                origPosition.x -= ((deltaTime - minTime) / (maxTime - minTime)) * (canvasRight - canvasLeft);
                go.transform.position = origPosition;
            }
            while (created.First.Value.transform.position.x < 0)
            {
                GameObject first = created.First.Value;
                created.RemoveFirst();
                Destroy(first);
            }
        }
    }

    private float timeRatio(float time)
    {
        return Mathf.Clamp((time - minTime) / (maxTime - minTime), 0, 1);
    }
    private float dbRatio(float db)
    {
        return (db - minDB) / (maxDB - minDB);
    }
    private float pitchRatio(float pitch)
    {
        return (pitch - minPitch) / (maxPitch - minPitch);
    }

    private Vector2 screenPointFromRatio(Vector2 ratio)
    {
        return new Vector2(canvasLeft + (canvasRight - canvasLeft) * ratio.x, canvasTop + (canvasBottom - canvasTop) * ratio.y);
    }
    public void CreateDBDataPoint(float dbData)
    {
        created.AddLast(Instantiate(dbDataPoint, screenPointFromRatio(new Vector2(timeRatio(time), dbRatio(dbData))), Quaternion.identity, canvas.transform));
    }
    public void CreatePitchDataPoint(float pitchData)
    {
        //if (pitchData > 300.0f) return;
        created.AddLast(Instantiate(pitchDataPoint, screenPointFromRatio(new Vector2(timeRatio(time), pitchRatio(pitchData))), Quaternion.identity, canvas.transform));
    }
}
