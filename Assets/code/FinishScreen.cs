using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishScreen : MonoBehaviour
{
    public Camera main;
    public Color winColor;
    public Color loseColor;
    
    //public TextMeshProUGUI buttonText;
    public TextMeshProUGUI endText;
    public TextMeshProUGUI buttonText;

    void Start()
    {
    }

    void Update()
    {
        if (GameControl.control.gameWin)
        {
            endText.text = "You Win!";
            buttonText.text = "Play Again";
            main.backgroundColor = winColor;
        }
        else
        {
            endText.text = "Game Over!";
            buttonText.text = "Try Again";
            main.backgroundColor = loseColor;
        }
    }
}
