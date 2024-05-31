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
    private FirebaseAuth _auth;
    private string userId;

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;

        GetCurrentUserId();

        FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId).Child("friends").OrderByChild("friendRequests").LimitToLast(6).ValueChanged += GetFriendsList;
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

    public void GetFriendsList(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        DataSnapshot snapshot = args.Snapshot;

        // Clear existing friend entries
        var _friendList = gameObject.GetComponentsInChildren<FriendEntry>(); // Assuming FriendEntry script on friends' GameObjects
        foreach (var item in _friendList)
        {
            Destroy(item.gameObject);
        }

        int i = 0;
        foreach (var friendSnapshot in snapshot.Children)
        {
            string friendId = friendSnapshot.Key;
            string usernameFriend = friendSnapshot.Child("username").Value.ToString();
            bool isConnected = bool.Parse(friendSnapshot.Child("isConnected").Value.ToString()); // Assuming isConnected is stored as a boolean

            var friendEntryGO = GameObject.Instantiate(_prefabFriend, transform);
            friendEntryGO.transform.position = new Vector2(friendEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
            friendEntryGO.GetComponent<FriendEntry>().SetLabels(usernameFriend, friendId, isConnected);

            i++;
        }
    }
}
