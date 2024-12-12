using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    private string playerDataFileName = "playerData.bin";
    [SerializeField]
    private PlayerData playerData;

    public void savePlayerData()
    {
        if (!File.Exists(playerDataFileName))
            File.Create(playerDataFileName);

        PlayerDataDTO playerDataDTO = new PlayerDataDTO {
            health = playerData.health,
            stamina = playerData.stamina,
            strenght = playerData.strenght,
            defence = playerData.defence,
            exp = playerData.exp,
            lvl = playerData.lvl
        };
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, playerDataFileName);

        FileStream saveFile = File.Create(path);

        formatter.Serialize(saveFile, playerDataDTO);

        saveFile.Close();
    }

    public void loadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, playerDataFileName);
        if (!File.Exists(path))
        {
            File.Create(playerDataFileName);
            return;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream saveFile = File.Open(path, FileMode.Open);

        PlayerDataDTO playerDataLoad = (PlayerDataDTO)formatter.Deserialize(saveFile);

        playerData.health = playerDataLoad.health;
        playerData.stamina = playerDataLoad.stamina;
        playerData.strenght = playerDataLoad.strenght;
        playerData.defence = playerDataLoad.defence;
        playerData.exp = playerDataLoad.exp;
        playerData.lvl = playerDataLoad.lvl;

        saveFile.Close();
    }

    public void StartNewGame()
    {
        if (!File.Exists(playerDataFileName))
            File.Create(playerDataFileName);

        PlayerDataDTO playerDataDTO = new PlayerDataDTO
        {
            health = 100,
            stamina = 100,
            strenght = 1,
            defence = 1,
            exp = 0,
            lvl = 0
        };
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath, playerDataFileName);

        FileStream saveFile = File.Create(path);

        formatter.Serialize(saveFile, playerDataDTO);

        saveFile.Close();
    }
}
