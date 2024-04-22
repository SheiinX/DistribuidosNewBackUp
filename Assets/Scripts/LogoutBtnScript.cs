using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogoutBtnScript : MonoBehaviour //, IPointerClickHandler
{
    [SerializeField]
    private string _sceneToLoad = "Home";

    [SerializeField]
    private Button _logOutBtn;

    private void Reset()
    {
        _logOutBtn = GetComponent<Button>();
    }

    private void Start()
    {
        _logOutBtn.onClick.AddListener(OnPointerClick);
    }

    public void  OnPointerClick(/*PointerEventData eventData*/)
    {
        FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene(_sceneToLoad);
    }
}
