using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using TMPro;

public class SignUpBtnScript : MonoBehaviour
{
    [SerializeField]
    private Button _signUpBtn;

    [SerializeField]
    private TMP_InputField _emailInputField;

    [SerializeField]
    private TMP_InputField _usernameInputField;

    [SerializeField]
    private TMP_InputField _passwordInputField;

    private DatabaseReference _mDatabaseRef;

    // Use a separate variable for user connection state
    private bool _userIsConnected = false;  // Default to disconnected

    private void Reset()
    {
        _signUpBtn = GetComponent<Button>();
        _emailInputField = GameObject.Find("InputFieldEmail").GetComponent<TMP_InputField>();
        _passwordInputField = GameObject.Find("InputFieldPassword").GetComponent<TMP_InputField>();
        _usernameInputField = GameObject.Find("InputFieldUsername").GetComponent<TMP_InputField>();
    }

    private void Start()
    {
        _signUpBtn.onClick.AddListener(HandleSignupButton);
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;

        // Check for existing user connection on app launch (optional)
        // You can implement logic here to handle reconnection or display a message
        // if the user was previously connected.
        // ...
    }

    private void HandleSignupButton()
    {
        StartCoroutine(RegisterUser());
    }

    private IEnumerator RegisterUser()
    {
        var auth = FirebaseAuth.DefaultInstance;

        var signUpTask = auth.CreateUserWithEmailAndPasswordAsync(_emailInputField.text, _passwordInputField.text);

        yield return new WaitUntil(() => signUpTask.IsCompleted);

        if (signUpTask.IsCanceled)
        {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled");
        }
        else if (signUpTask.IsFaulted)
        {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + signUpTask.Exception);
        }
        else
        {
            Firebase.Auth.AuthResult result = signUpTask.Result;
            Debug.LogError($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");

            _mDatabaseRef.Child("users").Child(result.User.UserId).Child("username").SetValueAsync(_usernameInputField.text);

            FirebaseDatabase.DefaultInstance.RootReference.Child(result.User.UserId).Child("isConected").SetValueAsync(_userIsConnected);
            Debug.Log($"User connection state: {_userIsConnected}");
        }
    }
}


