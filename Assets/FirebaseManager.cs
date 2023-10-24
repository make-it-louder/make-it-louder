using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Auth;
using System;
using Firebase;
using Firebase.Database;

public class FirebaseManager
{
    private static FirebaseManager instance = null;
    private Firebase.DependencyStatus dependencyStatus;
    private FirebaseAuth auth;
    private FirebaseUser user;
    private DatabaseReference databaseReference;

    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseManager();
            }
            return instance;
        }
    }

    public void InitializeFirebase()
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
                auth = FirebaseAuth.DefaultInstance;
                if (auth.CurrentUser != null)
                {
                    SignOut();
                }
                auth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);

                databaseReference = FirebaseDatabase.GetInstance(FirebaseApp.DefaultInstance, "https://c102-30105-default-rtdb.firebaseio.com/").RootReference;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
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

    public void SignUp(string email, string username, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("failed to sign up");
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;

            Debug.LogError("successfully signed up");

            WriteNewUser(newUser.UserId, username);

            Debug.LogError("successfully writed up");
        });
    }

    public void SignIn(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("failed to sign in");
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            Debug.LogError("successfully signed in");
        });
    }

    public void SignOut()
    {
        auth.SignOut();
        Debug.LogError("successfully signed out");
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
}
