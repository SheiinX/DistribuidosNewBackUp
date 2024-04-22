using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveScoreButton : MonoBehaviour
{
    [SerializeField]
    private Button _saveScoreButton;

    [SerializeField]
    private Text _scoreText;

    private void Reset()
    {
        _saveScoreButton = GetComponent<Button>();
        _scoreText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _saveScoreButton.onClick.AddListener(HandlerSaveScoreButtonClicked);
    }

    private void HandlerSaveScoreButtonClicked()
    {
        int score = int.Parse(_scoreText.text);
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(uid).Child("score").SetValueAsync(score);
    }
}
