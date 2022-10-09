using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGame_button : MonoBehaviour
{
    public void StartNewGame()
    {
        GameManager.instance.newGame();
    }
}