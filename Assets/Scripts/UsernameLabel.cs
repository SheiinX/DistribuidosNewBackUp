using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsernameLabel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;

    private void Reset()
    {
        _label = GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(object sender, EventArgs e)
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

        if(currentUser != null)
        {
            SetLabelUsername(currentUser.UserId);
        }
    }

    private void SetLabelUsername(string username)
    {
        FirebaseDatabase.DefaultInstance.GetReference($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/username")
            .GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot);

                    _label.text = (string)snapshot.Value;
                }
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
