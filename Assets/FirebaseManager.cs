using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;
using System.Threading.Tasks; // Needed for the Unwrap extension method.
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager instance = null;

    private Firebase.DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference databaseReference;
    //����Ƽ ������Ʈ
    public GameObject signupForm;
    public GameObject loginForm;
    public GameObject popupWinodow;
    public TMP_Text popupTitle;
    public TMP_Text popupContent;

    public static FirebaseManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Firebase.FirebaseApp.CheckDependenciesAsync().ContinueWith(checkTask =>
        {
            Firebase.DependencyStatus status = checkTask.Result;
            if (status != Firebase.DependencyStatus.Available)
            {
                return Firebase.FirebaseApp.FixDependenciesAsync().ContinueWith(t =>
                {
                    return Firebase.FirebaseApp.CheckDependenciesAsync();
                }).Unwrap();
            }
            else
            {
                return checkTask;
            }
        }).Unwrap().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.LogError("All firebase dependencies have been resolved.: " + dependencyStatus);
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    public void InitializeFirebase()
    {
        //
        auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            SignOut();
        }
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        //
        databaseReference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance, "https://c102-30105-default-rtdb.firebaseio.com/").RootReference;
    }

    // Set authentication state change event handler and get user data
    private void AuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null && auth.CurrentUser.IsValid();
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out");
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in");
            }
        }
    }

    public async void SignUp(string email, string username, string password)
    {
        bool flag = false;
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("failed to sign up");
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            Debug.LogError("successfully signed up");
            flag = true;
            WriteNewUser(newUser.UserId, username);
            Debug.LogError("successfully writed up");
        });

        if (flag)
        {
            loginForm.SetActive(true);
            signupForm.SetActive(false);
        }
        else
        {
            popupTitle.text = "실패";
            popupContent.text = "입력한 정보를 확인해주세요!";
            popupWinodow.SetActive(true);

        }
    }

    public async void SignIn(string email, string password)
    {
        bool flag = false;
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("failed to sign in");
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            Debug.LogError("successfully signed in");
            flag = true;
        });
        if (flag) {
            SceneManager.LoadScene("Lobby");

        } else
        {
            popupTitle.text = "실패";
            popupContent.text = "ID/PW를 확인해주세요!";
            popupWinodow.SetActive(true);
        }
    }

    public void SignOut()
    {
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }

    public class User
    {
        public string username;

        public User()
        {
        }

        public User(string username)
        {
            this.username = username;
        }
    }

    private void WriteNewUser(string userId, string username)
    {
        User user = new User(username);
        string json = JsonUtility.ToJson(user);

        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
    public void ClosePopup()
    {
        popupWinodow.SetActive(false);
    }
}