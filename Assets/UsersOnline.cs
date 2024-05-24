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

    // Start is called before the first frame update
    void Start()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").LimitToLast(3).ValueChanged += HandleValueChanged;
        GetUsersOnline();
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
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
    }

    private void GetUsersOnline()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").LimitToLast(3).GetValueAsync()
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
                    string username = userObject["username"].ToString(); // Assuming username exists

                    Debug.Log(username);

                    var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
                    connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                    connectionEntryGO.GetComponent<RequestEntry>().SetLabels(username); // Assuming ConnectionEntry has a SetLabel method for username only

                    i++;
                }
            }
        });
    }
}

