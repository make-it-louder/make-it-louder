
using UnityEngine;

public class TempFirebase : MonoBehaviour
{
    private void Awake()
    {
        if (!FirebaseManager.Instance.IsLoggedIn())
        {
            FirebaseManager.Instance.SignIn("test@test.com", "asdfasdf", a);
        } else
        {
            Debug.Log("로그인한 아이디로 정상적으로 접속완료");
            Destroy(gameObject);
        }
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
