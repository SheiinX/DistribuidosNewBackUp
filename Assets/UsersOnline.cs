using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersOnline : MonoBehaviour // Changed class name to reflect functionality
{
    [SerializeField]
    private GameObject connectionEntryPrefab;

    [SerializeField]
    private float _spacedBoard;

    [SerializeField]
    private int limitLastList;

    // Start is called before the first frame update
    void Start()
    {
        //FirebaseDatabase.DefaultInstance.GetReference("users").LimitToLast(3).ValueChanged += HandleValueChanged;
        //GetUsersOnline();
        FirebaseDatabase.DefaultInstance.GetReference("users").ValueChanged += GetUsersFireBase;
    }

    /*private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        DataSnapshot snapshot = args.Snapshot;

        var _connectionBoard = gameObject.GetComponentsInChildren<RequestEntry>(); // Assuming you have a ConnectionEntry script
        foreach (var item in _connectionBoard)
        {
            Destroy(item.gameObject);
        }

        int i = 0;
        foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
        {
            var userObject = (Dictionary<string, object>)userDoc.Value;
            string username = userObject["username"].ToString(); // Assuming username exists

            Debug.Log(username);

            var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
            connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
            connectionEntryGO.GetComponent<RequestEntry>().SetLabels(username); // Assuming ConnectionEntry has a SetLabel method for username only

            i++;
        }
    }*/

    private void GetUsersFireBase(object sender, ValueChangedEventArgs args)
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

        int i = 0;
        foreach (var friendRequestDoc in snapshot.Children)
        {
            // Get user ID of the sender
            string usersOnlineId = friendRequestDoc.Key;
            Debug.Log($"entered foreach {usersOnlineId}");
            // Get user data based on friend ID
            FirebaseDatabase.DefaultInstance.GetReference("users")
                .Child(usersOnlineId)
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
                            Debug.Log($"User id of this {i} is {usersOnlineId}");
                            string username = userSnapshot.Child("username").Value.ToString(); // Assuming username exists

                            Debug.Log(username);

                            var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
                            connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                            connectionEntryGO.GetComponent<RequestEntry>().SetLabels(username, usersOnlineId);

                            i++;
                        }
                    }
                });
        }
    }
    /*
    private void GetUsersOnline()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").LimitToLast(limitLastList).GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log(task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Value);

                int i = 0;
                foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
                {
                    var userObject = (Dictionary<string, object>)userDoc.Value;
                    string userId = userDoc.Key;
                    Debug.Log($"User id of this {i} is {userId}");
                    string username = userObject["username"].ToString(); // Assuming username exists

                    Debug.Log(username);

                    var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
                    connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                    connectionEntryGO.GetComponent<RequestEntry>().SetLabels(username, userId); // Assuming ConnectionEntry has a SetLabel method for username only

                    i++;
                }
            }
        });
    }*/
}

