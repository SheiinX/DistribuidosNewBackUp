using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsersConected : MonoBehaviour
{
    [SerializeField]
    private GameObject connectionEntryPrefab;

    [SerializeField]
    private float _spacedBoard;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("isConected").LimitToLast(3).ValueChanged += HandleValueChanged;
        GetUsersConnectionStatus();
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
        foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
        {
            var userObject = (Dictionary<string, object>)userDoc.Value;
            string connectionStatus = userObject["isConected"].ToString(); // Assuming isConected is a boolean stored as a string in Firebase

            Debug.Log($"{userObject["username"]} : {connectionStatus}");

            var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
            connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
            connectionEntryGO.GetComponent<ConnectionEntry>().SetLabel($"{userObject["username"]}", connectionStatus); // Assuming ConnectionEntry has a SetLabel method

            i++;
        }
    }

    private void GetUsersConnectionStatus()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("isConected").LimitToLast(3).GetValueAsync()
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
                        string connectionStatus = userObject["isConected"].ToString();

                        Debug.Log($"{userObject["username"]} : {connectionStatus}");

                        var connectionEntryGO = GameObject.Instantiate(connectionEntryPrefab, transform);
                        connectionEntryGO.transform.position = new Vector2(connectionEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                        connectionEntryGO.GetComponent<ConnectionEntry>().SetLabel($"{userObject["username"]}", connectionStatus);

                        i++;
                    }
                }
            });
    }
}

