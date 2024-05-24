using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;

public class CreateInStartUserList : MonoBehaviour
{

    private DatabaseReference _databaseRef;
    private FirebaseAuth _auth;

    private void Start()
    {
        _databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;

        GetUserAuth();
    }

    private void GetUserAuth()
    {
        FirebaseUser currentUser = _auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is authenticated.");
            return;
        }

        string currentUserId = currentUser.UserId;
        Debug.Log(currentUserId);
        InitializeFriendsList(currentUserId);
        InitializeFriendRequestList(currentUserId);
    }

    private void InitializeFriendsList(string userId)
    {
        _databaseRef.Child("users").Child(userId).Child("friends").SetValueAsync(new Dictionary<string, object>()).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friends list initialized successfully.");
            }
            else
            {
                Debug.LogError("Failed to initialize friends list: " + task.Exception);
            }
        });
    }

    private void InitializeFriendRequestList(string userId)
    {
        _databaseRef.Child("users").Child(userId).Child("friendRequests").SetValueAsync(new Dictionary<string, object>()).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friends list initialized successfully.");
            }
            else
            {
                Debug.LogError("Failed to initialize friends list: " + task.Exception);
            }
        });
    }
    
}
