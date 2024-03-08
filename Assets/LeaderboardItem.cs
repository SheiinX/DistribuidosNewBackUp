using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardItem : MonoBehaviour
{
    public string Username { get; private set; }
    public int Score { get; private set; }
    public int Position { get; private set; }

    public TMP_Text textUsername;
    public TMP_Text textScore;

    public void SetItem(JsonUser user, int position)
    {
        textUsername.text = user.username;
        textScore.text = "" + user.data.score;

        transform.position = new Vector3(0, 100 - (position * 100), 0);
    }

    // Start is called before the first frame update
    void Start()
    {

    }
}
