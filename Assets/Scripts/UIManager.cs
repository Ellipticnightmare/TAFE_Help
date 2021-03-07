using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : GameManager
{
    [HideInInspector]
    public Image healthBar, manaBar, staminaBar; //Images that represent our stats
    [HideInInspector]
    public GameObject pauseMenu; //Pause Menu parent object
    public static bool isPaused; //Determine if paused
    private void Update()
    {
        healthBar.fillAmount = (float)base.playerHealth / 100; //Update healthbar Display
        manaBar.fillAmount = (float)base.playerMana / 100; //Update manabar Display
        staminaBar.fillAmount = (float)base.playerStamina / 100; //Update staminabar Display

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause(); //Pause and Unpause when you hit the Escape Key
    }
    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy); //Disable and enable the pause menu from its previous state
        bool isActive = pauseMenu.activeInHierarchy; //Create bool equal to new pause menu state
        isPaused = isActive; //Detect if paused
        if (isActive)
            base.PauseGame(); //Run pause function
        else
            base.UnPauseGame(); //Run unpause function
    }
    public void PlayGame(string inString)
    {
        SceneManager.LoadScene(inString); //Load scene from defined string
    }
}