using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private Piece pieceBehaviour;
    [SerializeField] private string controlButton;


    public void OnMouseDown()
    {

        pieceBehaviour.controlString = controlButton;

        /*
        switch (controlButton)
        {
            case "Right":
                //pieceBehaviour.HandleMoveInputsButtons(1);
                pieceBehaviour.controlString = controlButton;
                break;
            case "Left":
                //pieceBehaviour.HandleMoveInputsButtons(2);
                break;
            case "Down":
                //pieceBehaviour.HandleMoveInputsButtons(3);
                break;
            case "HardDrop":
                //pieceBehaviour.HandleMoveInputsButtons(4);
                break;
            case "RLeft":
                //pieceBehaviour.HandleMoveInputsButtons(5);
                break;
            case "RRight":
                //pieceBehaviour.HandleMoveInputsButtons(6);
                break;
        }*/
    }
}
