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

    // Start is called before the first frame update
    void Start()
    {
        // Get current user ID (replace with your logic)
        _currentUserId = "your_user_id"; // Example placeholder

        // Listen for changes in friend requests for the current user
        FirebaseDatabase.DefaultInstance.GetReference("users")
            .Child(_currentUserId)
            .Child("friendRequests")
            .ValueChanged += HandleFriendRequestsChanged;
    }

    private void HandleFriendRequestsChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        DataSnapshot snapshot = args.Snapshot;

        // Clear existing entries
        var connectionEntries = GetComponentsInChildren<RequestEntry>();
        foreach (var entry in connectionEntries)
        {
            Destroy(entry.gameObject);
        }

        int i = 0;
        foreach (var friendRequestDoc in snapshot.Children)
        {
            // Get user ID of the sender
            string friendId = friendRequestDoc.Key;

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

                        if (userSnapshot.Exists)
                        {
                            // Extract username (assuming it exists)
                            string username = userSnapshot.Child("username").Value.ToString();

                            // Create entry for the user
                            var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
                            connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                            connectionEntryGO.GetComponent<RequestEntry>().SetLabels(username); // Assuming SetLabels takes username

                            i++;
                        }
                    }
                });
        }
    }
}



