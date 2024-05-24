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

    public int Score { get; private set; }

    private LeaderboardHandler leaderboard;

    //public int Score { get; private set; }

    [SerializeField] GameObject panelControl;
    [SerializeField] GameObject tetrisGamePanel;

    [SerializeField] TextMeshPro playerName;
    [SerializeField] TextMeshProUGUI scorePlayer;

    [SerializeField] TextMeshProUGUI tokenInformation;

    [SerializeField] GameObject logOutButton;

    [SerializeField] Board boardInformation;

    [SerializeField] GameObject yesButton;
    [SerializeField] GameObject textScore;

    private AuthenticationData data;

    private int userScore;

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
            Debug.Log(Username);
            StartCoroutine("GetProfile");
        }
    }

    private void Update()
    {
        ScoreReceiver();
    }

    public void ScoreReceiver()
    {
        userScore = boardInformation.currentScore;
    }

    public void SendRegister()
    {
        /*AuthenticationData*/ data = new AuthenticationData();

        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine("Register", JsonUtility.ToJson(data));
    }

    public void SendLogin()
    {
        /*AuthenticationData*/ data = new AuthenticationData();

        data.username = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>().text;
        data.password = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>().text;

        StartCoroutine("Login", JsonUtility.ToJson(data));
    }

    public void SendLogOut()
    {
        Token = "";
        Username = "";

        PlayerPrefs.SetString("token", Token);
        PlayerPrefs.SetString("username", Username);

        tetrisGamePanel.SetActive(false);
        textScore.SetActive(false);
        yesButton.SetActive(false);
        panelControl.SetActive(true);
        logOutButton.SetActive(false);

        Debug.Log("Pulsado y expulsado");
    }

    public void SendScore()
    {
        JsonUser data = new JsonUser();

        data.data = new DataUser();
        data.data.score = userScore;
        /*
        data.usuario = new JsonUser();
        data.usuario.data = new DataUser();
        data.usuario.data.score = userScore;*/


        //data.username = this.data.usuario.username;

        Score = data.data.score;
        PlayerPrefs.SetInt("score", Score);

        //scorePlayer.text = Score.ToString();
        scorePlayer.text = data.data.score.ToString();

        Debug.Log($"The actual score is {data.data.score}");

        StartCoroutine("UpdateScore", JsonUtility.ToJson(data));
    }

    IEnumerator UpdateScore(string json)
    {
        UnityWebRequest request = UnityWebRequest.Put(url + "/api/usuarios/", json);
        request.method = "PATCH";
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("x-token", Token);
        

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

                Debug.Log($"New user score is {data.usuario.data.score}");
            }
            else
            {
                Debug.Log(request.responseCode + "|" + request.error);
            }
        }
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
                Username = data.usuario.username;
                //Score = data.user.data.score;

                PlayerPrefs.SetString("token", Token);
                PlayerPrefs.SetString("username", Username);

                panelControl.SetActive(false);
                tetrisGamePanel.SetActive(true);
                logOutButton.SetActive(true);

                textScore.SetActive(true);
                yesButton.SetActive(true);

                leaderboard.TakeLeaderboard();

                playerName.text = Username;

                Debug.Log(data.token);
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
        Debug.Log(url + "/api/usuarios/" + Username);
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

                Debug.Log($"User{data.usuario.username} is authenticated");
                panelControl.SetActive(false);
                tetrisGamePanel.SetActive(true);
                logOutButton.SetActive(true);

                textScore.SetActive(true);
                yesButton.SetActive(true);

                playerName.text = data.usuario.username;
                scorePlayer.text = data.usuario.data.score.ToString();
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
    public JsonUser usuario;
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
    public bool isConected;
}


