using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using TMPro;
using UnityEngine.UI;

public class FriendRequest : MonoBehaviour
{
    [SerializeField]
    private Button _cancelButton, _acceptButton;

    [SerializeField]
    private TMP_Text _usernameText; // Campo de texto para mostrar el nombre del usuario

    private DatabaseReference _mDatabaseRef;
    private string _userId; // Identificador del usuario actual
    private string _friendId; // Identificador del amigo
    private string _friendName; // Nombre del amigo
    private string _friendEmail; // Correo electrónico del amigo (opcional)

    private void Awake()
    {
        _acceptButton = GetComponent<Button>();
        _cancelButton = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        _acceptButton.onClick.AddListener(AcceptFriend);
        _cancelButton.onClick.AddListener(DeclineFriend);

        // Obtener identificadores de usuario y amigo (desde parámetros o lógica de la aplicación)
    }

    private void AcceptFriend()
    {
        // Agregar el amigo a la lista de amigos del usuario actual
        AddFriend(_userId, _friendId, _friendName, _friendEmail);
    }

    private void DeclineFriend()
    {
        // Eliminar la solicitud de amistad (opcional)
    }

    private void AddFriend(string userId, string friendId, string friendName, string friendEmail)
    {
        User friendUser = new User(friendName, friendEmail);
        string json = JsonUtility.ToJson(friendUser);

        _mDatabaseRef.Child("users").Child(userId).Child("friends").Child(friendId).SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Friend added successfully.");
            }
            else
            {
                Debug.LogError("Failed to add friend: " + task.Exception);
            }
        });
    }

    [System.Serializable]
    public class User
    {
        public string username;
        public string email;

        public User(string username, string email)
        {
            this.username = username;
            this.email = email;
        }
    }
}

