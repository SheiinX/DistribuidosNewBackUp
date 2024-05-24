using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using UnityEngine.UI;

public class FriendRequest : MonoBehaviour
{
    [SerializeField]
    private Button _cancelButton, _acceptButton;

    private DatabaseReference _mDatabaseRef;

    private void Awake()
    {
        _acceptButton = GetComponent<Button>();
        _cancelButton = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        //_acceptButton.onClick.AddListener(AcceptDeclineRequest());
    }

    private void AcceptDeclineRequest(string userId, string friendId, string name, string email)
    {
        User friendUser = new User(name, email);
        string json = JsonUtility.ToJson(friendUser);

        _mDatabaseRef.Child("users").Child(userId).Child("friends").Child(friendId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friend added successfully.");
            }
            else
            {
                Debug.LogError("Failed to add friend: " + task.Exception);
            }
        });
    }

    /*
     public void AddFriend(string userId, string friendId, string friendUsername, int friendScore)
    {
        Friend newFriend = new Friend(friendUsername, friendScore);
        string json = JsonUtility.ToJson(newFriend);

        dbReference.Child("users").Child(userId).Child("friends").Child(friendId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friend added successfully.");
            }
            else
            {
                Debug.LogError("Failed to add friend: " + task.Exception);
            }
        });
    }
     */
}

/*
 [System.Serializable]
public class Friend
{
    public string username;
    public int score;

    public Friend(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}
 */

[System.Serializable]
public class User
{
    public string username;
    public string email;

    public User(string username, string email)
    {
        this.username = username;
        this.email = email;
    }
}
