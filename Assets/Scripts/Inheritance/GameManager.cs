using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public int playerHealth, playerMana, playerStamina, playerLevel, playerEXP;
    public virtual void GameQuit()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        Debug.Log("Paused Game");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void UnPauseGame()
    {
        Debug.Log("Unpaused Game");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}