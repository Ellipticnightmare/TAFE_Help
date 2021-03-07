using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int playerHealth, playerMana, playerStamina; //Display stats
    public virtual void GameQuit() //Quits the game
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        Debug.Log("Paused Game");
        Cursor.visible = true; //Hides cursor
        Cursor.lockState = CursorLockMode.Confined; //Confines cursor to game window
    }
    public void UnPauseGame()
    {
        Debug.Log("Unpaused Game");
        Cursor.visible = false; //Hides cursor
        Cursor.lockState = CursorLockMode.Locked; //Confined cursor to center of screen
    }
}