using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;

public class FriendRequest : MonoBehaviour
{
    [SerializeField]
    private Button _cancelButton, _acceptButton;

    [SerializeField]
    private TMP_Text _usernameText; // Campo de texto para mostrar el nombre del usuario

    private DatabaseReference _mDatabaseRef;
    private FirebaseAuth _auth;
    private string _userId; // Identificador del usuario actual
    private string _friendId; // Identificador del amigo
    private string _friendName; // Nombre del amigo

    private RequestEntry requestEntry;

    private void Awake()
    {
        requestEntry = GetComponent<RequestEntry>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
        _auth = FirebaseAuth.DefaultInstance;

        // Initialize the friendId and friendName
        InitializeFriendData();

        _acceptButton.onClick.AddListener(AcceptFriend);
        _cancelButton.onClick.AddListener(DeclineFriend);

        // Obtener identificadores de usuario y amigo (desde parámetros o lógica de la aplicación)
    }

    private void InitializeFriendData()
    {
        FirebaseUser currentUser = _auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is authenticated.");
            return;
        }

        _userId = currentUser.UserId;

        // Assuming that requestEntry has the friend ID and friend name
        _friendId = requestEntry._uid;
        Debug.Log($"id for accept/deny request {_friendId}");
        _friendName = requestEntry.NameOfTheUser();
        Debug.Log($"username for accept/deny request {_friendId}");

        if (string.IsNullOrEmpty(_friendId) || string.IsNullOrEmpty(_friendName))
        {
            Debug.LogError("Friend ID or name is not properly initialized.");
        }
    }

    private void AcceptFriend()
    {
        if (string.IsNullOrEmpty(_friendId) || string.IsNullOrEmpty(_friendName))
        {
            Debug.LogError("Friend ID or name is not set.");
            return;
        }

        // Agregar el amigo a la lista de amigos del usuario actual
        Debug.Log($"{_friendId}");
        Debug.Log($"{_friendName}");
        AddFriend(_friendId, _friendName);
    }

    private void DeclineFriend()
    {
        // Eliminar la solicitud de amistad (opcional)
        RemoveFriendRequest(_friendId);
    }

    private void AddFriend(string friendId, string friendName)
    {
        FirebaseUser currentUser = _auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is authenticated.");
            return;
        }

        string currentUserId = currentUser.UserId;

        User friendUser = new User(friendName, friendId);
        string json = JsonUtility.ToJson(friendUser);

        _mDatabaseRef.Child("users").Child(currentUserId).Child("friends").Child(friendId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friend added successfully.");
                // Optionally remove the friend request
                RemoveFriendRequest(friendId);
            }
            else
            {
                Debug.LogError("Failed to add friend: " + task.Exception);
            }
        });
    }

    private void RemoveFriendRequest(string friendId)
    {
        FirebaseUser currentUser = _auth.CurrentUser;
        if (currentUser == null)
        {
            Debug.LogError("No user is authenticated.");
            return;
        }

        string currentUserId = currentUser.UserId;

        _mDatabaseRef.Child("users").Child(currentUserId).Child("friendRequests").Child(friendId).RemoveValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friend request removed successfully.");
            }
            else
            {
                Debug.LogError("Failed to remove friend request: " + task.Exception);
            }
        });
    }
}

[System.Serializable]
public class User
{
    public string username;
    public string uid;

    public User(string username, string uid)
    {
        this.username = username;
        this.uid = uid;
    }
}



