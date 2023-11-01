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
    public GameObject loadingSpinner;

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
                Debug.LogError("All firebase dependencies have been resolved: " + dependencyStatus);
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

    //
    public class Profile
    {
        public string username;
        public string avatar;
        public List<string> avatars;
        public List<string> achievements;

        public Profile()
        {
        }

        public Profile(string username, string avatar, List<string> avatars, List<string> achievements)
        {
            this.username = username;
            this.avatar = avatar;
            this.avatars = avatars;
            this.achievements = achievements;
        }
    }

    //
    public class Record
    {
        public int playtime;
        public int count_jump;
        public int count_fall;

        public Record()
        {
        }

        public Record(int playtime, int count_jump, int count_fall)
        {
            this.playtime = playtime;
            this.count_jump = count_jump;
            this.count_fall = count_fall;
        }
    }

    // sign up new users
    public async void SignUp(string email, string password, string username, Action<bool> callback)
    {
        bool flag = false;

        loadingSpinner.SetActive(true);

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
            loadingSpinner.SetActive(false);
            callback(flag);
        }
    }

    // sign in existing users
    public async void SignIn(string email, string password, Action<bool> callback)
    {
        bool flag = false;

        loadingSpinner.SetActive(true);

        try
        {
            Firebase.Auth.AuthResult result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.LogError("successfully signed in");

            flag = true;
        }
        catch (Exception e)
        {
            Debug.LogError("failed to sign in: " + e.Message);
        }
        finally
        {
            loadingSpinner.SetActive(false);
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
        string defaultAvatar = "avatar1";
        List<string> defaultAvatars = new List<string>() { "avatar1"};
        List<string> defaultAchievements = new List<string>() { "achievement1"};

        Profile user = new Profile(username, defaultAvatar, defaultAvatars, defaultAchievements);
        string json = JsonUtility.ToJson(user);

        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

    //
    private void WriteRecord(string userId)
    {
        Dictionary<string, Record> records = new Dictionary<string, Record>
        {
            { "map1", new Record(0, 0, 0) },
            { "map2", new Record(0, 0, 0) },
            { "map3", new Record(0, 0, 0) }
        };

        string json = JsonConvert.SerializeObject(records, Formatting.Indented);
        databaseReference.Child("records").Child(userId).SetRawJsonValueAsync(json);
    }

    //
    public async Task<Profile> GetProfile(string userId)
    {
        var task = databaseReference.Child("users").Child(userId).GetValueAsync();
        await task;

        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                Profile user = JsonConvert.DeserializeObject<Profile>(json);
                return user;
            }
            else
            {
                Debug.LogError("The user data does not exist");
                return null;
            }
        }
        else
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve user data from the database due to a faulted state");
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("Failed to retrieve user data from the database due to a canceled state");
            }
            return null;
        }
    }

    //
    public async Task<Dictionary<string, Record>> GetRecords(string userId)
    {
        var task = databaseReference.Child("records").Child(userId).GetValueAsync();
        await task;

        if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                Dictionary<string, Record> records = JsonConvert.DeserializeObject<Dictionary<string, Record>>(json);
                Debug.Log(records["map1"].count_jump);
                return records;
            }
            else
            {
                Debug.LogError("The record data does not exist");
                return null;
            }
        }
        else
        {
            if (task.IsFaulted)
            {
                Debug.LogError("failed to retrieve from the database due to a faulted state");
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("failed to retrieve from the database due to a canceled state");
            }
            return null;
        }
    }
}
