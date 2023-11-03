using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempGoToGame : MonoBehaviour
{
    public TMP_Text cntText;
    Dictionary<string, FirebaseManager.Record> record;

    // Start is called before the first frame update
    void Start()
    {
        record = RecordManager.Instance.UserRecords;
        cntText.text = record["map1"].count_jump.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("MakeItLouder");
    }

    public void GetCount()
    {

    }
}
