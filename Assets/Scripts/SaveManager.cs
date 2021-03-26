using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public InputField playerNameReader;
    public Dropdown playerNameSelectDropdown;
    public List<string> playerNames = new List<string>();
    private void Start()
    {
        ReadFromFile();
    }
    public void CreatePlayerName()
    {
        PlayerPrefs.SetString("curPlayerName", playerNameReader.text);
        AddToFile();
    }
    public void UpdatePlayerName(int value)
    {
        PlayerPrefs.SetString("curPlayerName", playerNameSelectDropdown.options[value].text);
    }
    public void ReadFromFile()
    {
        if (File.Exists(Application.persistentDataPath + "/players.database"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/players.database", FileMode.Open);
            playerDatabase dataB = (playerDatabase)bf.Deserialize(file);
            file.Close();
            playerNames = dataB.playerNamesDatabase;
        }
        else
        {
            playerNames.Add("Jeffrey");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/players.database");
            playerDatabase dataB = new playerDatabase();
            dataB.playerNamesDatabase = playerNames;
            bf.Serialize(file, dataB);
            file.Close();
        }
        playerNameSelectDropdown.ClearOptions();
        playerNameSelectDropdown.AddOptions(playerNames);
    }
    public void AddToFile()
    {
        if (!playerNames.Contains(playerNameReader.text))
        {
            playerNames.Add(playerNameReader.text);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/players.database");

            playerDatabase dataB = new playerDatabase();
            dataB.playerNamesDatabase = playerNames;

            bf.Serialize(file, dataB);
            file.Close();
            ReadFromFile();
        }
    }
}