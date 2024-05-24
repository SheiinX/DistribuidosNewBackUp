using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserListLabel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _labelUsername;

    public void SetLabels(string username)
    {
        _labelUsername.text = username;
    }
}
