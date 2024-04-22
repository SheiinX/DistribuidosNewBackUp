using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPanelBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject _panelToActivate;

    [SerializeField]
    private Button _btnToPress;

    private void Reset()
    {
        _btnToPress = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _btnToPress.onClick.AddListener(ActivatePanel);
    }

    private void ActivatePanel()
    {
        _panelToActivate.SetActive(true);
    }
}
