using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Extensions;

public class FriendSendRequest : MonoBehaviour
{
    [SerializeField]
    private Button _sendRequestButton;

    [SerializeField]
    private TMP_Text username;

    private DatabaseReference _mDatabaseRef;
    private FirebaseAuth _auth;
    private RequestEntry requestUser;

    private string usernameGot;
    private string idGot;

    private void Awake()
    {
        requestUser = GetComponent<RequestEntry>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;

        //Debug.Log($"The id of the request is {requestUser._uid}");
        _sendRequestButton.onClick.AddListener(() => BridgeToSendRequest());
    }

    private void BridgeToSendRequest()
    {
        GetCurrentUserName();
    }

    private void GetCurrentUserName()
    {
        FirebaseUser currentUser = _auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is authenticated.");
            return;
        }

        idGot = currentUser.UserId;

        //string currentUserUsername = "";

        _mDatabaseRef.Child("users").Child(idGot).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot userSnapshot = task.Result;

                if (userSnapshot.Exists)
                {
                    usernameGot = userSnapshot.Child("username").Value.ToString();
                    SendFriendRequest(requestUser._uid, usernameGot, idGot);
                    Debug.Log($"Username of the current user is {usernameGot}");
                }
            }
        });

        Debug.Log($"Username outside of the if is {usernameGot}");
    }

    private void SendFriendRequest(string friendId, string currentUsername, string currentId)
    {

        if (currentUsername == "")
        {
            Debug.Log("No username got it");
            return;
        }

        if (currentId == "")
        {
            Debug.Log("No id got it");
            return;
        }

        FriendRequestData requestData = new FriendRequestData(currentId, currentUsername);
        string json = JsonUtility.ToJson(requestData);

        _mDatabaseRef.Child("users").Child(friendId).Child("friendRequests").Child(currentId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friend request sent successfully.");
                _sendRequestButton.gameObject.SetActive(false);
                Debug.Log($"{friendId}");
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
