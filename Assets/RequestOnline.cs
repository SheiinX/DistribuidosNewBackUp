using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestOnline : MonoBehaviour
{
    [SerializeField]
    private GameObject connectionEntryPrefab;

    [SerializeField]
    private float _spacedBoard;

    private string _currentUserId; // Assuming you have a way to get the current user ID
    private DatabaseReference _mDatabaseRef;
    private FirebaseAuth _auth;

    // Start is called before the first frame update
    void Start()
    {
        // Get current user ID (replace with your logic)
        GetCurrentUserId();

        // Listen for changes in friend requests for the current user
        FirebaseDatabase.DefaultInstance.GetReference("users")
            .Child(_currentUserId)
            .Child("friendRequests")
            .ValueChanged += HandleFriendRequestsChanged;
    }

    private void GetCurrentUserId()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;

        FirebaseUser currentUser = _auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is authenticated.");
            return;
        }

        _currentUserId = currentUser.UserId;
    }

    private void HandleFriendRequestsChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        DataSnapshot snapshot = args.Snapshot;
        Debug.Log("Got Snapshot");

        string jsonSnapshot = snapshot.GetRawJsonValue();
        Debug.Log("Snapshot JSON: " + jsonSnapshot);

        // Clear existing entries
        var connectionEntries = GetComponentsInChildren<RequestEntry>();
        Debug.Log($"Number of RequestEntry components found: {connectionEntries.Length}");
        foreach (var entry in connectionEntries)
        {
            Destroy(entry.gameObject);
            Debug.Log("Entering Destroy function");
        }

        int i = 0;
        foreach (var friendRequestDoc in snapshot.Children)
        {
            // Get user ID of the sender
            string friendId = friendRequestDoc.Key;
            Debug.Log("entered foreach");
            // Get user data based on friend ID
            FirebaseDatabase.DefaultInstance.GetReference("users")
                .Child(friendId)
                .GetValueAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.LogError(task.Exception);
                        return;
                    }

                    if (task.IsCompleted)
                    {
                        DataSnapshot userSnapshot = task.Result;
                        Debug.Log("Got user snapshot");
                        if (userSnapshot.Exists)
                        {
                            string userJsonSnapshot = userSnapshot.GetRawJsonValue();
                            Debug.Log("User Snapshot JSON: " + userJsonSnapshot);

                            // Extract username (assuming it exists)
                            string username = userSnapshot.Child("username").Value.ToString();
                            Debug.Log($"Name of the user friend is {username}");
                            string userId = friendId;
                            Debug.Log($"Id of the user friend is {userId}");

                            // Create entry for the user
                            var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
                            connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                            connectionEntryGO.GetComponent<RequestEntry>().SetLabels(username, userId); // Assuming SetLabels takes username

                            i++;
                        }
                    }
                });
        }
    }
}



