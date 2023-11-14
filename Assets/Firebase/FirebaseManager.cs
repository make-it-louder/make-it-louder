using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Auth;
using System.Threading.Tasks;
using Firebase;
using Firebase.Database;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class FirebaseManager : MonoBehaviour
{
    // singleton
    private static FirebaseManager instance = null;

    // firebase
    private Firebase.DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference databaseReference;

    // interface

    public static FirebaseManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        InitializeFirebase();

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Debug.Log("인스턴스 중복임 Destroy");
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
                Debug.Log("All firebase dependencies have been resolved");
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
            auth.SignOut();
        }
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);

        // get a databasereference
        databaseReference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance, "https://c102-52393-default-rtdb.asia-southeast1.firebasedatabase.app/").RootReference;
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

    //
    public class Profile
    {
        public string username;
        public int e_avatar;
        public List<bool> avatars;

        public Profile()
        {
        }

        public Profile(
            string username,
            int e_avatar,
            List<bool> avatars)
        {
            this.username = username;
            this.e_avatar = e_avatar;
            this.avatars = avatars;
        }
    }

    //
    public class Record
    {
        public float playtime;
        public int count_jump;
        public int count_fall;
        public int count_clear;
        public int count_minjump;
        public float min_cleartime;

        public Record()
        {
        }

        public Record(float playtime, int count_jump, int count_fall, int count_clear, int count_minjump, float min_cleartime)
        {
            this.playtime = playtime;
            this.count_jump = count_jump;
            this.count_fall = count_fall;
            this.count_clear = count_clear;
            this.count_minjump = count_minjump;
            this.min_cleartime = min_cleartime;
        }
    }
    public class Ranking
    {
        public Dictionary<string, float> addicter { get; set; }
        public Dictionary<string, float> cleartime { get; set; }
        public Dictionary<string, int> min_jump { get; set; }
        public Dictionary<string, int> max_jump { get; set; }

        public Ranking()
        {
            addicter = new Dictionary<string, float>();
            cleartime = new Dictionary<string, float>();
            min_jump = new Dictionary<string, int>();
            max_jump = new Dictionary<string, int>();
        }


    }

    //
    public DatabaseReference GetDatabaseReference()
    {
        return databaseReference;
    }

    // isLogined?
    public bool IsLoggedIn()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        return auth.CurrentUser != null;
    }

    //닉네임 중복검사
    public async Task<bool> IsUsernameTaken(string username)
    {
        var snapshot = await databaseReference.Child("users").OrderByChild("username").EqualTo(username).GetValueAsync();
        return snapshot.Exists && snapshot.ChildrenCount > 0;
    }
    // sign up new users
    public async void SignUp(string email, string password, string username, Action<bool> callback)
    {
        bool flag = false;
        try
        {
            Firebase.Auth.AuthResult result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            Debug.LogError("successfully signed up");

            WriteUser(result.User.UserId, username);
            Debug.LogError("user has been successfully written");

            WriteRecord(result.User.UserId);
            Debug.LogError("record has been successfully written");

            flag = true;
        }
        catch (Exception e)
        {
            Debug.LogError("failed to sign up: " + e.Message);
        }
        finally
        {
            callback(flag);
        }
    }

    // sign in existing users
    public async void SignIn(string email, string password, Action<bool> callback)
    {
        bool flag = false;


        try
        {
            Firebase.Auth.AuthResult result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.Log("successfully signed in");

            await RecordManager.Instance.GetUser(result.User.UserId);
            flag = true;
        }
        catch (Exception e)
        {
            Debug.LogError("failed to sign in: " + e.Message);
        }
        finally
        {
            callback(flag);
        }
    }

    // sign out a user
    public void SignOut()
    {
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }

    //
    private void WriteUser(string userId, string username)
    {
        int defaultAvatar = 0;
        List<bool> defaultAvatars = new List<bool>() { true, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        List<string> defaultAchievements = new List<string>() { "achievement1" };

        Profile user = new Profile(username, defaultAvatar, defaultAvatars);
        string json = JsonUtility.ToJson(user);

        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    //
    private void WriteRecord(string userId)
    {
        Dictionary<string, Record> records = new Dictionary<string, Record>
        {
            { "map1", new Record(0f, 0, 0, 0, 0, 0f) },
            { "map2", new Record(0f, 0, 0, 0, 0, 0f) },
            { "map3", new Record(0f, 0, 0, 0, 0, 0f) }
        };

        string json = JsonConvert.SerializeObject(records, Formatting.Indented);
        databaseReference.Child("records").Child(userId).SetRawJsonValueAsync(json);
    }
}
