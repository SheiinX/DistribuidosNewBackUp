using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;

public class LoginBtnScript : MonoBehaviour
{
    [SerializeField]
    private Button _loginUpBtn;

    [SerializeField]
    private TMP_InputField _emailInputField;

    [SerializeField]
    private TMP_InputField _passwordInputField;

    private void Reset()
    {
        _loginUpBtn = GetComponent<Button>();
        _emailInputField = GameObject.Find("InputFieldEmail").GetComponent<TMP_InputField>();
        _passwordInputField = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>();

    }

    private void Start()
    {
        _loginUpBtn.onClick.AddListener(HandleSignupButton);
    }

    private void HandleSignupButton()
    {
        var auth = FirebaseAuth.DefaultInstance;

        var loginUpTask = auth.SignInWithEmailAndPasswordAsync(_emailInputField.text, _passwordInputField.text);

        if(loginUpTask.IsCanceled)
        {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled");
        }
        else if(loginUpTask.IsFaulted)
        {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error " + loginUpTask.Exception);
        }
        else
        {
            Firebase.Auth.AuthResult result = loginUpTask.Result;
            Debug.LogError($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");
        }
    }
}
