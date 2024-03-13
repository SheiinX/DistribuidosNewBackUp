using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class LeaderboardHandler : MonoBehaviour
{
    private string url = "https://sid-restapi.onrender.com";

    private JsonUser User;
    private string Token;

    public GameObject leaderboardPanel;
    public GameObject leaderboardItemPrefab;

    private List<GameObject> leaderboardItems;


    // Start is called before the first frame update
    void Start()
    {
        Token = PlayerPrefs.GetString("token");
        leaderboardItems = new List<GameObject>();
    }

    public void TakeLeaderboard()
    {
        StartCoroutine("GetLeaderboard");
    }

    

    IEnumerator GetLeaderboard()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "/api/usuarios");
        request.method = "PATCH";
        Debug.Log("Sending Leaderboard GetProfile");
        request.SetRequestHeader("x-token", Token);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            //Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                UserList data = JsonUtility.FromJson<UserList>(request.downloadHandler.text);

                var organizedUsers = data.users.OrderByDescending(u => u.data.score).Take(5).ToArray();

                ShowLeaderboard(organizedUsers);
            }
            else
            {
                Debug.Log($"{request.responseCode}|{request.error}");
            }
        }
    }

    private void ShowLeaderboard(JsonUser[] users)
    {
        leaderboardPanel.SetActive(true);
        leaderboardItems.Clear();


        foreach(JsonUser user in users)
        {
            GameObject item = GameObject.Instantiate(leaderboardItemPrefab, leaderboardPanel.transform) as GameObject;

            SetLeaderboard(item, user);
        }
    }

    /*public void HideLeaderboar()
    {
        leaderboardPanel.SetActive(false);
    }*/

    private void SetLeaderboard(GameObject item, JsonUser user)
    {
        leaderboardItems.Add(item);
        LeaderboardItem leaderboardItem = item.GetComponent<LeaderboardItem>();

        leaderboardItem.SetItem(user, leaderboardItems.Count());
    }
}
