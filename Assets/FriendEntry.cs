using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FriendEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _usernameText;

    [SerializeField]
    private TMP_Text _statusText;

    private string _friendId;
    private bool _isConnected;

    // M�todo para establecer las etiquetas de la entrada de amigo
    public void SetLabels(string username, string friendId, bool isConnected)
    {
        _usernameText.text = username;
        _friendId = friendId;
        _isConnected = isConnected;

        // Actualizar el estado de conexi�n
        UpdateConnectionStatus();
    }

    // M�todo para actualizar el estado de conexi�n
    private void UpdateConnectionStatus()
    {
        _statusText.text = _isConnected ? "Connected" : "Offline";
    }

    // Otros m�todos p�blicos para manejar interacciones con la entrada de amigo, si es necesario
}


