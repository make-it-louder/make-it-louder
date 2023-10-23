using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ForceExitTest : MonoBehaviour
{
    public GameObject gameObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnApplicationQuit()
    {
        string filePath = Application.persistentDataPath + "/lastPosition.txt";

        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine($"OnQuit: last position={gameObject.transform.position}");
        }
    }
}
