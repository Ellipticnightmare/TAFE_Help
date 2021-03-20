using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public InputField playerNameReader;
    public virtual void UpdatePlayerName()
    {
        PlayerPrefs.SetString("curPlayerName", playerNameReader.text);
    }
    public virtual void GameQuit() //Quits the game
    {
        Application.Quit();
    }
    public void LoadNewScene(string inSceneName)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
        {
            PlayerController quickCheck = FindObjectOfType<PlayerController>();
            quickCheck.SaveData();
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(inSceneName);
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
[System.Serializable]
public class KeybindData
{
    public string keyBindName, keybindData;
}
[System.Serializable]
public class dataString
{
    public KeybindData[] keybinds;
}
[System.Serializable]
public class playerLevelData
{
    public int maxHealth, maxMana, maxStamina, expToNextLevel;
}
[System.Serializable]
public class playerReadData
{
    public string data;
}
[System.Serializable]
public class playerSettingsData
{
    public float volume, mouseSensitivityX, mouseSensitivityY;
}