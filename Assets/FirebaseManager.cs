using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    // firebase
    private static FirebaseManager instance = null;

    private Firebase.DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference databaseReference;

    // unity
    public GameObject signupForm;
    public GameObject loginForm;
    public GameObject popupWinodow;
    public TMP_Text popupTitle;
    public TMP_Text popupContent;

    // write, update, or delete data at a reference
    private User userdata;

    public GameObject loadingSpinner;

    public class User
    {
        public string username;
        public string avatar;
        public List<string> avatars;
        public List<string> achievements;

        public User()
        {
        }

        public User(string username, string avatar, List<string> avatars, List<string> achievements)
        {
            this.username = username;
            this.avatar = avatar;
            this.avatars = avatars;
            this.achievements = achievements;
        }
    }

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
        // check and fix dependenciesAsync
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
        // returns the firebaseAuth
        auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            SignOut();
        }
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        // get a databasereference
        databaseReference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance, "https://c102-30105-default-rtdb.firebaseio.com/").RootReference;
    }

    // set an authentication state change event handler and get user data
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

    // sign up new users
    public async void SignUp(string email, string username, string password)
    {
        bool flag = false;

        loadingSpinner.SetActive(true);
        
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("failed to sign up");
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogError("successfully signed up");
            
            WriteNewUser(result.User.UserId, username);
            Debug.LogError("successfully writed up");

            flag = true;
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

        loadingSpinner.SetActive(false);
    }

    public async void SignIn(string email, string password)
    {
        bool flag = false;
        loadingSpinner.SetActive(true);
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

        loadingSpinner.SetActive(false);
    }

    public void SignOut()
    {
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }

    private void WriteNewUser(string userId, string username)
    {
        string defaultAvatar = "avatar1";
        List<string> defaultAvatars = new List<string>() { "avatar1"};
        List<string> defaultAchievements = new List<string>() { "achievement1"};

        User user = new User(username, defaultAvatar, defaultAvatars, defaultAchievements);
        string json = JsonUtility.ToJson(user);

        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }
    public void ClosePopup()
    {
        popupWinodow.SetActive(false);
    }
}