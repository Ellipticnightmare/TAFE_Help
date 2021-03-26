using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : GameManager
{
    [HideInInspector] //Our Character Controller, which moves and manipulates the player
    public CharacterController chara;
    [HideInInspector] //Our floats which detect and influence how the Character Controller behaves
    public float horizontalDetect, verticalDetect, speed;
    public Camera mainCam; //Our camera
    public movementState curMove = movementState.walk; //A state controller to determine movement speeds and other variables
    string playerName;
    #region PlayerStats
    public int playerLevel, playerEXP, playerGold;
    public static float playerHealth, playerMana, playerStamina, playerSpeed; //Display stats
    public static int playerMaxHealth, playerMaxMana, playerMaxStamina; //Display stats
    public playerLevelData[] levelData;
    string filePath;
    #endregion
    public Potion activePotion;
    Potion effectPotion;
    public Potion[] potionsInInventory;
    bool hasEffectActive;
    float potionEffectTimer;
    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        playerName = PlayerPrefs.GetString("curPlayerName");
        filePath = Application.persistentDataPath + "/" + playerName + "/" + playerName;
        LoadData();
        Cursor.visible = false; //Hides cursor
        Cursor.lockState = CursorLockMode.Locked; //Confined cursor to center of screen
        mainCam = Camera.main; //Locate Camera
        if (mainCam == null)
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //Backup Locate Camera
        if (playerLevel <= 1)
            playerLevel = 1; //Set player level
        chara = GetComponent<CharacterController>(); //Find Character Controller Variable
    }
    private void Update()
    {
        if (UIManager.isPaused == false) //Determine if game is paused
        {
            #region Status Effects
            if (hasEffectActive)
            {
                if (potionEffectTimer > 0)
                    potionEffectTimer -= Time.deltaTime;
                else
                {
                    hasEffectActive = false;
                }
                foreach (var item in effectPotion.EffectGain)
                {
                    switch (item)
                    {
                        case Potion.effect.recovery:
                            playerHealth += effectPotion.healVal * Time.deltaTime;
                            break;
                        case Potion.effect.recharge:
                            playerMana += effectPotion.healVal * Time.deltaTime;
                            break;
                        case Potion.effect.catchyourbreath:
                            playerStamina += effectPotion.healVal * Time.deltaTime;
                            break;
                        case Potion.effect.blank:
                            break;
                    }
                }
            }
            else
            {
                int tempVal = 0;
                tempVal = Mathf.RoundToInt(playerStamina);
                playerStamina = tempVal;
                tempVal = Mathf.RoundToInt(playerMana);
                playerMana = tempVal;
                tempVal = Mathf.RoundToInt(playerHealth);
                playerHealth = tempVal;
            }
            #endregion
            if (playerStamina < playerMaxStamina)
                playerStamina += Time.deltaTime * (.5f * playerLevel);
            if (playerMana < playerMaxMana)
                playerMana += Time.deltaTime * (.35f * playerLevel);
            if (playerHealth < playerMaxHealth)
                playerHealth += Time.deltaTime * (.15f * playerLevel);
            if (Input.GetKeyDown(KeyCode.Tab))
                playerEXP += 500;
            Debug.Log(playerMana);
            playerMaxHealth = levelData[playerLevel - 1].maxHealth;
            playerMaxMana = levelData[playerLevel - 1].maxMana;
            playerMaxStamina = levelData[playerLevel - 1].maxStamina;
            if (playerEXP >= levelData[playerLevel - 1].expToNextLevel)
            {
                playerEXP -= levelData[playerLevel - 1].expToNextLevel;
                StartCoroutine(IncreaseLevel());
            }
            //Register keybindings from playerPrefs
            KeyCode jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump"));
            KeyCode crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch"));
            KeyCode run = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Run"));
            KeyCode shoot = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Shoot"));
            //KeyCode potion = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Potion"));
            if (Input.GetKey(run)) //run
                curMove = movementState.run;
            else if (Input.GetKey(crouch)) //crouch
                curMove = movementState.crouch;
            else //default to walk
                curMove = movementState.walk;
            if (Input.GetKeyDown(jump)) //Test Jump
                Debug.Log("Ki");
            if (Input.GetKeyDown(shoot))
            {
                UsePotion();
            }
            switch (curMove) //Adjust stats based off of current movement type
            {
                case movementState.walk:
                    speed = playerSpeed * 1;
                    break;
                case movementState.crouch:
                    speed = playerSpeed * .85f;
                    break;
                case movementState.run:
                    speed = playerSpeed * 2.25f;
                    break;
            }
            horizontalDetect = Input.GetAxis("Horizontal"); //Take in A + D input
            verticalDetect = Input.GetAxis("Vertical"); //Take in S + W input
            Vector3 move = this.transform.TransformDirection(horizontalDetect, 0, verticalDetect);
            chara.Move(move * Time.deltaTime * speed); //Move character controller

            this.gameObject.transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X"), 0)); //Rotate left to right
            mainCam.transform.Rotate(new Vector3(-(Input.GetAxisRaw("Mouse Y")), 0, 0)); //Look up and down
        }
    }
    public void LoadData()
    {
        Debug.Log(playerName);
        string[] rawData;
        if (File.Exists(filePath + ".playerData"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath + ".playerData", FileMode.Open);
            playerReadData check = (playerReadData)bf.Deserialize(file);
            file.Close();
            rawData = check.data.Split('|');
            #region readData
            playerLevel = int.Parse(rawData[0]);
            playerEXP = int.Parse(rawData[1]);
            playerGold = int.Parse(rawData[2]);
            playerHealth = float.Parse(rawData[3]);
            playerMana = float.Parse(rawData[4]);
            playerStamina = float.Parse(rawData[5]);
            playerSpeed = levelData[playerLevel].speed;
            #endregion
            #region readSettings
            FileStream settingsFile = File.Open(filePath + ".playerSettings", FileMode.Open);
            playerSettingsData setCheck = (playerSettingsData)bf.Deserialize(settingsFile);
            settingsFile.Close();
            Settings.volumeControl = setCheck.volume;
            #endregion
        }
        else
        {
            #region Directory Management
            Directory.CreateDirectory(Application.persistentDataPath + "/" + playerName);
            #endregion
            playerLevel = 1;
            playerEXP = 0;
            playerGold = 10;
            playerHealth = 100;
            playerMana = 100;
            playerStamina = 50;
            Settings.volumeControl = -40;
            Settings.mouseSensY = 5;
            Settings.mouseSensX = 5;
            SaveData();
        }
    }
    public void SaveData()
    {
        string dataSave = "";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath + ".playerData");
        #region saveData
        dataSave += playerLevel + "|";
        dataSave += playerEXP + "|";
        dataSave += playerGold + "|";
        dataSave += playerHealth + "|";
        dataSave += playerMana + "|";
        dataSave += playerStamina + "|";
        #endregion
        #region dataSave
        playerReadData check = new playerReadData();
        check.data = dataSave;
        bf.Serialize(file, check);
        file.Close();
        #endregion
        #region settingsSave
        FileStream setFile = File.Create(filePath + ".playerSettings");
        playerSettingsData setCheck = new playerSettingsData();
        setCheck.mouseSensitivityX = Settings.mouseSensX;
        setCheck.mouseSensitivityY = Settings.mouseSensY;
        setCheck.volume = Settings.volumeControl;
        bf.Serialize(setFile, setCheck);
        setFile.Close();
        #endregion
    }
    public void UsePotion()
    {
        if (activePotion != null)
        {
            foreach (var item in activePotion.StatToRecover)
            {
                switch (item)
                {
                    case Potion.stat.blank:
                        break;
                    case Potion.stat.health:
                        playerHealth += activePotion.healVal;
                        break;
                    case Potion.stat.mana:
                        playerMana += activePotion.healVal;
                        break;
                    case Potion.stat.stamina:
                        playerStamina += activePotion.healVal;
                        break;
                    case Potion.stat.speed:
                        if (activePotion.healVal > 9)
                        {
                            int speedUp = Mathf.RoundToInt(activePotion.healVal / 100) + 1;
                            playerSpeed += speedUp;
                        }
                        else
                            playerSpeed += activePotion.healVal;
                        break;
                }
            }
            foreach (var item in activePotion.StatToDamage)
            {
                switch (item)
                {
                    case Potion.stat.blank:
                        break;
                    case Potion.stat.health:
                        playerHealth -= activePotion.healVal;
                        break;
                    case Potion.stat.mana:
                        playerMana -= activePotion.healVal;
                        Debug.Log("Damaging Mana");
                        break;
                    case Potion.stat.stamina:
                        playerStamina -= activePotion.healVal;
                        break;
                    case Potion.stat.speed:
                        playerSpeed -= activePotion.healVal;
                        break;
                }
            }
            if (activePotion.effectTimer > 0)
            {
                hasEffectActive = true;
                potionEffectTimer = activePotion.effectTimer;
            }
            effectPotion = activePotion;
            //Remove from inventory
            activePotion = null;
        }
    }
    IEnumerator IncreaseLevel()
    {
        playerLevel++;
        Debug.Log("Leveled up!");
        float healthRatio = playerHealth / playerMaxHealth;
        float manaRatio = playerMana / playerMaxMana;
        float staminaRatio = playerStamina / playerMaxStamina;
        yield return new WaitForSeconds(.01f);
        playerHealth = playerMaxHealth * healthRatio;
        playerMana = playerMaxMana * manaRatio;
        playerStamina = playerMaxStamina * staminaRatio;
        SaveData();
    }

    public enum movementState //Categorize the different movement options
    {
        walk,
        crouch,
        run
    };
}