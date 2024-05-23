using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;

public class FriendsHandler : MonoBehaviour
{

    private DatabaseReference _mDatabaseRef;

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void GetFriendsList(string userId)
    {
        _mDatabaseRef.Child("users").Child(userId).Child("friends").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot friendSnapshot in snapshot.Children)
                {
                    string friendId = friendSnapshot.Key;
                    string friendUsername = friendSnapshot.Child("username").Value.ToString();
                    Debug.Log($"Friend ID: {friendId}, Username: {friendUsername}");
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve friends list: " + task.Exception);
            }
        });
    }

    /*
    public void GetFriendsList(string userId)
    {
        _mDatabaseRef.Child("users").Child(userId).Child("friends").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot friendSnapshot in snapshot.Children)
                {
                    string friendId = friendSnapshot.Key;
                    string friendUsername = friendSnapshot.Child("username").Value.ToString();
                    int friendScore = int.Parse(friendSnapshot.Child("score").Value.ToString());
                    Debug.Log($"Friend ID: {friendId}, Username: {friendUsername}, Score: {friendScore}");
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve friends list: " + task.Exception);
            }
        });
    }*/
}
