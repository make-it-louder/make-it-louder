
using UnityEngine;

public class TempFirebase : MonoBehaviour
{
    private void Awake()
    {
        FirebaseManager.Instance.SignIn("test@test.com", "asdfasdf", a);
    }

    public void a(bool flag) 
    { 
        if (flag)
        {
            Debug.Log("Test아이디로 로그인성공, RecordManager에도 연동완료.");
        } 
        else
        {
            Debug.Log("Test아이디로 로그인 실패");
        }
    }
        
}