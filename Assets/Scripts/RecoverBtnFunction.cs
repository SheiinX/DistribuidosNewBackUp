using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecoverBtnFunction : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _emailInputField;

    [SerializeField]
    private Button _sendEmailRecover;

    [SerializeField]
    private GameObject _panelToDeActivate;
    [SerializeField]
    private GameObject _panelToActivate;

    private void Reset()
    {
        _sendEmailRecover = GetComponent<Button>();
        _emailInputField = GameObject.Find("EmailInputRecover").GetComponent<TMP_InputField>();
        _panelToDeActivate = GameObject.Find("Recover");
        _panelToActivate = GameObject.Find("StartSession");
    }

    // Start is called before the first frame update
    void Start()
    {
        _sendEmailRecover.onClick.AddListener(SendEmailForRecoverPassword);
    }

    private void SendEmailForRecoverPassword()
    {
        Debug.Log("clicked");
        var auth = FirebaseAuth.DefaultInstance;
        //FirebaseUser user = auth.CurrentUser;

        string emailAddress = _emailInputField.text;
        //if (user != null)
        //{
            auth.SendPasswordResetEmailAsync(emailAddress).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    return;
                }

                _panelToActivate.SetActive(true);
                _panelToDeActivate.SetActive(false);
                Debug.Log("Password reset email sent successfully.");
            });
        //}
    }
}
