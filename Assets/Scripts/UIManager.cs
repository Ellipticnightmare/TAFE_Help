using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : GameManager
{
    [HideInInspector]
    public Image healthBar, manaBar, staminaBar;
    [HideInInspector]
    public GameObject pauseMenu;
    public static bool isPaused;
    private void Update()
    {
        healthBar.fillAmount = (float)base.playerHealth / 100;
        manaBar.fillAmount = (float)base.playerMana / 100;
        staminaBar.fillAmount = (float)base.playerStamina / 100;

        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }
    public void TogglePause()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        bool isActive = pauseMenu.activeInHierarchy;
        if (isActive)
            base.PauseGame();
        else
            base.UnPauseGame();
    }
}