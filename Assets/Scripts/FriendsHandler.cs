using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using Firebase.Auth;

public class FriendsHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabFriend;

    [SerializeField]
    private float _spacedBoard;

    private DatabaseReference _mDatabaseRef;
    private string userId;

    // Start is called before the first frame update
    void Start()
    {
        //_mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        //FirebaseAuth user = FirebaseAuth.DefaultInstance;
        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        userId = user.UserId;

        FirebaseDatabase.DefaultInstance.GetReference(userId).OrderByChild("friends").LimitToLast(6).ValueChanged += GetFriendsList;
    }

    public void GetFriendsList(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        DataSnapshot snapshot = args.Snapshot;

        var _friendList = gameObject.GetComponentsInChildren<RequestEntry>();
        foreach (var item in _friendList)
        {
            Destroy(item.gameObject);
        }

        int i = 0;
        foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
        {
            var userObject = (Dictionary<string, object>)userDoc.Value;
            string userId = snapshot.Key;

            var scoreEntryGO = GameObject.Instantiate(_prefabFriend, transform);
            scoreEntryGO.transform.position = new Vector2(scoreEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
            scoreEntryGO.GetComponent<RequestEntry>().SetLabels($"{userObject["username"]}", userId);

            i++;
        }
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
        });*/

    /*_mDatabaseRef.Child("users").Child(userId).Child("friends").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                int i = 0;
                foreach (DataSnapshot friendSnapshot in snapshot.Children)
                {
                    string friendId = friendSnapshot.Key;
                    string friendUsername = friendSnapshot.Child("username").Value.ToString();

                    var friendEntryBoard = GameObject.Instantiate(_prefabFriend, transform);
                    friendEntryBoard.transform.position = new Vector2(friendEntryBoard.transform.position.x, transform.position.y - i * _spacedBoard);
                    
                    i++;
                }
            }
            else
            {
                Debug.LogError("Failed to retrieve friends list: " + task.Exception);
            }
        });
    }*/
}
