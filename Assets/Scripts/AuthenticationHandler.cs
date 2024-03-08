using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class AuthenticationHandler : MonoBehaviour
{
    //Username: SheiinX
    //Password: NeverGonnaGiveYouUp
    private string url = "https://sid-restapi.onrender.com";

    public string Token { get; private set; }

    public string Username { get; private set; }

    private LeaderboardHandler leaderboard;

    //public int Score { get; private set; }

    [SerializeField] GameObject panelControl;
    [SerializeField] GameObject tetrisGamePanel;

    [SerializeField] TextMeshPro playerName;
    [SerializeField] TextMeshPro scorePlayer;

    [SerializeField] TextMeshProUGUI tokenInformation;

    void Start()
    {
        leaderboard = GetComponent<LeaderboardHandler>();

        Token = PlayerPrefs.GetString("token");

        if (string.IsNullOrEmpty(Token))
        {
            Debug.Log("There's no token");
            panelControl.SetActive(true);
        }
        else
        {
            Username = PlayerPrefs.GetString("username");
            StartCoroutine("GetProfile");
        }
    }

    public void SendRegister()
    {
        AuthenticationData data = new AuthenticationData();

        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine("Register", JsonUtility.ToJson(data));
    }

    public void SendLogin()
    {
        AuthenticationData data = new AuthenticationData();

        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine("Login", JsonUtility.ToJson(data));
    }

    IEnumerator Register(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios/", json);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                Debug.Log("Successful Register");
                StartCoroutine("Login", json);
            }
            else
            {
                //Debug.Log($"{request.responseCode}|{request.error}");
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator Login(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/auth/login/", json);
        request.method = "POST";
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.downloadHandler.text);

            if (request.responseCode == 200)
            {
                AuthenticationData data = JsonUtility.FromJson<AuthenticationData>(request.downloadHandler.text);

                Token = data.token;
                Username = data.user.username;
                //Score = data.user.data.score;

                PlayerPrefs.SetString("token", Token);
                PlayerPrefs.SetString("username", Username);

                panelControl.SetActive(false);
                tetrisGamePanel.SetActive(true);

                leaderboard.TakeLeaderboard();

                playerName.text = Username;

                Debug.Log(data.token);

                tokenInformation.text = request.downloadHandler.text;
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
    }

    IEnumerator GetProfile()
    {
        UnityWebRequest request = UnityWebRequest.Get(url + "/api/usuarios/" + Username);
        Debug.Log("Sending request GetProfile");
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
                AuthenticationData data = JsonUtility.FromJson<AuthenticationData>(request.downloadHandler.text);

                Debug.Log($"User{data.username} is authenticated");
                Debug.Log($"User{data.user.username} is authenticated and their score is {data.user.data.score}");
                //GameObject.Find("Panel").SetActive(false);

                playerName.text = data.user.username;
                scorePlayer.text = data.user.data.score.ToString();

                JsonUser[] users = new JsonUser[10];

                JsonUser[] organizedUser = users.OrderByDescending(user => user.data.score).ToArray();
                //users.Where(user => user._id == "12345").ToList();
            }
            else
            {
                Debug.Log("The user is not authentificated");
            }
        }
    }
}
[System.Serializable]
public class AuthenticationData
{
    public string username;
    public string password;
    public JsonUser user;
    public string token;
}

[System.Serializable]
public class JsonUser
{
    public string _id;
    public string username;
    public DataUser data;
}

[System.Serializable]
public class UserList
{
    public JsonUser[] users;
}

[System.Serializable]
public class DataUser
{
    public int score;
}


