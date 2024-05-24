using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequestEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _labelUsername;

    public string _uid;


    public void SetLabels(string username, string userId)
    {
        _labelUsername.text = username;
        _uid = userId;
    }
    
    public string NameOfTheUser()
    {
        return _labelUsername.text;
    }
}
