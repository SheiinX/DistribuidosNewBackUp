using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLeaderboardAssign : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreEntryPrefab;

    [SerializeField]
    private float _spacedBoard;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").LimitToLast(3).ValueChanged += HandleValueChanged;
        //GetUsersHighestScores();
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if(args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        DataSnapshot snapshot = args.Snapshot;

        var _leaderboard = gameObject.GetComponentsInChildren<ScoreEntry>();
        foreach (var item in _leaderboard)
        {
            Destroy(item.gameObject);
        }

        int i = 0;
        foreach (var userDoc in (Dictionary<string, object>)snapshot.Value)
        {
            var userObject = (Dictionary<string, object>)userDoc.Value;
            Debug.Log($"{userObject["username"]} : {userObject["score"]}");

            var scoreEntryGO = GameObject.Instantiate(scoreEntryPrefab, transform);
            scoreEntryGO.transform.position = new Vector2(scoreEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
            scoreEntryGO.GetComponent<ScoreEntry>().SetLabels($"{userObject["username"]}", $"{userObject["score"]}");

            i++;
        }
    }

    private void GetUsersHighestScores()
    {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").LimitToLast(3).GetValueAsync()
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
                    Debug.Log($"{userObject["username"]} : {userObject["score"]}");

                    var scoreEntryGO = GameObject.Instantiate(scoreEntryPrefab, transform);
                    scoreEntryGO.transform.position = new Vector2(scoreEntryGO.transform.position.x, transform.position.y - i * _spacedBoard);
                        scoreEntryGO.GetComponent<ScoreEntry>().SetLabels($"{userObject["username"]}", $"{userObject["score"]}");

                        i++;
                    }
                }
            });
    }
}
