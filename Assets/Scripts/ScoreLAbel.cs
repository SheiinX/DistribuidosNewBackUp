using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreLAbel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _label;

    private void Reset()
    {
        _label = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        FirebaseDatabase.DefaultInstance.GetReference($"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}/score")
            .ValueChanged += HandleValueChanged;
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if(args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        Debug.Log(args.Snapshot);

        string _score = (string)args.Snapshot.Value;
        int score = string.IsNullOrEmpty(_score) ? 0 : int.Parse(_score);

        _label.text = "Score: " + score;
    }
}
