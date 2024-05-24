using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectionEntry : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _labelUsername;

    [SerializeField]
    private TMP_Text _labelScore;

    public void SetLabel(string username, string score)
    {
        _labelUsername.text = username;
        _labelScore.text = score;
    }
}
