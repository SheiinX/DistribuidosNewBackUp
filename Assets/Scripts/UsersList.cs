using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class UsersList : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefabUserList;

    [SerializeField]
    private float _spacedBoard;

    private DatabaseReference _mDatabaseRef;

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        GetUserList();
    }

    public void GetUserList()
    {
        int i = 0;

        _mDatabaseRef.Child("users").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot userSnapshot in snapshot.Children)
                {
                    string userId = userSnapshot.Key;
                    string username = userSnapshot.Child("username").Value.ToString();

                    var userEntryGO = GameObject.Instantiate(_prefabUserList, transform);
                    userEntryGO.transform.position = new Vector2(userEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                    userEntryGO.GetComponent<UserListLabel>().SetLabels(username);

                }
            }
            else
            {
                Debug.LogError("Failed to retrieve users: " + task.Exception);
            }
        });
    }
}
