using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FriendSendRequest : MonoBehaviour
{
    [SerializeField]
    private Button _sendRequestButton;

    [SerializeField]
    private TMP_Text username;

    private DatabaseReference _mDatabaseRef;
    private FirebaseAuth _auth;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;

        _sendRequestButton.onClick.AddListener(() => SendFriendRequest("friendId_example"));
    }

    private void SendFriendRequest(string friendId)
    {
        FirebaseUser currentUser = _auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is authenticated.");
            return;
        }

        string currentUserId = currentUser.UserId;
        FriendRequestData requestData = new FriendRequestData(currentUserId, currentUser.DisplayName);
        string json = JsonUtility.ToJson(requestData);

        _mDatabaseRef.Child("users").Child(friendId).Child("friendRequests").Child(currentUserId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friend request sent successfully.");
            }
            else
            {
                Debug.LogError("Failed to send friend request: " + task.Exception);
            }
        });
    }
}

[System.Serializable]
public class FriendRequestData
{
    public string senderId;
    public string senderName;

    public FriendRequestData(string senderId, string senderName)
    {
        this.senderId = senderId;
        this.senderName = senderName;
    }
}
