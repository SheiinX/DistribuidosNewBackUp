using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersConnected1 : MonoBehaviour
{
    [SerializeField]
    private GameObject connectionEntryPrefab;

    [SerializeField]
    private float _spacedBoard;

    private DatabaseReference _mDatabaseRef;
    private FirebaseAuth _auth;
    private string userId;

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;
        GetCurrentUserId();

        // Subscribe to the friend list updates instead of all users
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId).Child("friends").OrderByChild("friendRequests").LimitToLast(3).ValueChanged += HandleValueChanged;
    }

    private void GetCurrentUserId()
    {
        FirebaseUser currentUser = _auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is authenticated.");
            return;
        }

        userId = currentUser.UserId;
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        DataSnapshot snapshot = args.Snapshot;

        var _connectionBoard = gameObject.GetComponentsInChildren<ConnectionEntry>(); // Assuming you have a ConnectionEntry script
        foreach (var item in _connectionBoard)
        {
            Destroy(item.gameObject);
        }

        int i = 0;
        foreach (var friendDoc in snapshot.Children)
        {
            string friendId = friendDoc.Key;

            _mDatabaseRef.Child("users").Child(friendId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot friendSnapshot = task.Result;
                    var userObject = (Dictionary<string, object>)friendSnapshot.Value;
                    string connectionStatus = userObject["isConected"].ToString(); // Assuming isConected is a boolean stored as a string in Firebase

                    Debug.Log($"{userObject["username"]} : {connectionStatus}");

                    var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
                    connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                    connectionEntryGO.GetComponent<ConnectionEntry>().SetLabel($"{userObject["username"]}", connectionStatus); // Assuming ConnectionEntry has a SetLabel method

                    i++;
                }
            });
        }
    }
}


