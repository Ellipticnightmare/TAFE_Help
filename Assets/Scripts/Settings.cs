using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Settings : GameManager
{
    public GameObject keybindUI; //Save the UI in settings
    public Button curKeyBind; //Adjust the current button
    public Button[] keyBindButtons; //Hold the buttons that are updated at the start
    bool isReadingForKey; //Begin listening for key data
    string curKeyToBind; //Set key to bind
    public List<KeybindData> keybindsMain = new List<KeybindData>(); //Hold current keybinds
    private void Start()
    {
        LoadData(); //Check if I have a save file
    }
    private void UpdateKeyBindUI()
    {
        foreach (var item in keybindsMain) //Iterate through keybinding list
        {
            foreach (var item2 in keyBindButtons) //Iterate through buttons list
            {
                if (item.keyBindName == item2.name) //If button matches keybinding entry, update text
                {
                    item2.GetComponentInChildren<Text>().text = item2.name + ": " + keyName(item.keybindData);
                    PlayerPrefs.SetString(item2.name, item.keybindData); //Update keybind data in playerPrefs
                }
            }
        }
    }
    public void toggleKeyBinds() //Hide and show keybinding UI
    {
        keybindUI.SetActive(!keybindUI.activeInHierarchy);
    }
    public override void GameQuit()
    {
        SaveData(); //Save current game data
        base.GameQuit(); //Quit application
    }
    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter(); //Open new BinaryFormatter
        FileStream file = File.Create(Application.persistentDataPath + "keybinds.data"); //Create file

        dataString check = new dataString(); //Add reference to data
        check.keybinds = keybindsMain.ToArray(); //Build reference so data is populated

        bf.Serialize(file, check); //Build data to file
        file.Close(); //Close file
    }
    public void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "keybinds.data")) //Check if file exists
        {
            BinaryFormatter bf = new BinaryFormatter(); //Open new BinaryFormatter
            FileStream file = File.Open(Application.persistentDataPath + "keybinds.data", FileMode.Open); //Open data file
            dataString check = (dataString)bf.Deserialize(file); //Read data by deserializing file
            file.Close(); //Close file
            keybindsMain.Clear(); //Clear keybinds
            keybindsMain.AddRange(check.keybinds); //Update keybinds from data
        }
        else
            Debug.Log("No File Found");
        UpdateKeyBindUI(); //Update Keybinding UI
    }
    public void StartReadKey(Button curButton)
    {
        isReadingForKey = true; //Start waiting for key input
        curKeyBind = curButton; //Set current button to update
        curKeyToBind = curButton.name.ToString(); //Get Key to re-register
    }
    string keyName(string inButton) //Update string of key based on mouse input
    {
        string output = null;
        if (int.TryParse(inButton, out int number)) //Detect if string is a mouse button (int)
        {
            switch (inButton)
            {
                case "0":
                    output = "Left Mouse";
                    break;
                case "1":
                    output = "Right Mouse";
                    break;
            }
        }
        else
            output = inButton;
        return output;
    }
    private void OnGUI()
    {
        Event e = Event.current; //Create new Event
        if (isReadingForKey)
        {
            if (e.isKey) //check for any key pressed
            {
                isReadingForKey = false; //Stop listening for key
                Debug.Log("Detected key code: " + e.keyCode); //Debug check
                curKeyBind.GetComponentInChildren<Text>().text = curKeyToBind + ": " + e.keyCode; //Update button text
                PlayerPrefs.SetString("" + curKeyToBind, e.keyCode.ToString()); //Update player prefs
                foreach (var item in keybindsMain) //Update keybinds Array for saving purposes
                {
                    if (item.keyBindName == curKeyToBind)
                        item.keybindData = e.keyCode.ToString();
                }
                SaveData(); //Save new keybindings to external file
            }
            else if (e.isMouse)
            {
                isReadingForKey = false; //Stop listening for key
                Debug.Log("Detected mouse: " + e.button); //Debug check
                curKeyBind.GetComponentInChildren<Text>().text = curKeyToBind + ": " + keyName(e.button.ToString()); //Update button text
                PlayerPrefs.SetString("" + curKeyToBind, e.button.ToString()); //Update player prefs
                foreach (var item in keybindsMain) //Update keybinds Array for saving purposes
                {
                    if (item.keyBindName == curKeyToBind)
                        item.keybindData = e.button.ToString();
                }
                SaveData(); //Save new keybindings to external file
            }
        }
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