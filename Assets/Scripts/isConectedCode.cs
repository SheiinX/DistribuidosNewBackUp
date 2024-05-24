using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class isConectedCode : MonoBehaviour
{
    [SerializeField]
    private Button _saveScoreButton;

    public bool _isConected;

    private void Reset()
    {
        _saveScoreButton = GetComponent<Button>();

    }

    // Start is called before the first frame update
    void Start()
    {
        _isConected = true;
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("isConected").SetValueAsync(_isConected);
        Debug.Log(_isConected);
        _saveScoreButton.onClick.AddListener(HandlerConectButtonClicked);
    }

    private void HandlerConectButtonClicked()
    {
        _isConected = false;
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("isConected").SetValueAsync(_isConected);
        Debug.Log(_isConected);
    }
}
