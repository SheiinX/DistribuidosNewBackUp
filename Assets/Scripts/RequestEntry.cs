using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequestEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _labelUsername;

    private string _uid;

    public string _Uid
    {
        get { return _uid; }
        set { _uid = value; }
    }

    public void SetLabels(string username, string userId)
    {
        _labelUsername.text = username;
        _uid = userId;
    }
}
