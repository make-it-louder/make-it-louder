using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Functions;
using System.Threading.Tasks;

public class FirebaseUserDelete : MonoBehaviour
{
    // Start is called before the first frame update
    private FirebaseFunctions functions;
    Dictionary<string, string> userInfo;
    void Start()
    {
        functions = FirebaseFunctions.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallFirebaseFunction()
    {
        // 클라우드 함수 호출
        CallFunction("deleteUserData").ContinueWith((task) =>
        {
            if (task.Exception != null)
            {
                Debug.LogError($"Failed to call function: {task.Exception}");
            }
            else
            {
                Debug.Log("Function call was successful");
            }
        });
    }

    private async Task CallFunction(string functionName)
    {
        // 클라우드 함수 호출 및 응답 처리
        try
        {
            var result = await functions.GetHttpsCallable(functionName).CallAsync();
            Debug.Log(result.Data);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error calling function {functionName}: {e.Message}");
        }
    }
}
