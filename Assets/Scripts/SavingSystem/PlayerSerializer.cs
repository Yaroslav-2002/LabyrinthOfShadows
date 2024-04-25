using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class PlayerSerializer
{
    private static string SavePath = Application.persistentDataPath + "player.save";

    public static void Serialize(PlayerData playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(SavePath, FileMode.Create))
        {
            formatter.Serialize(stream, playerData);
        }
    }

    public static PlayerData Deserialize()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogError("Save file not found.");
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(SavePath, FileMode.Open))
        {
            return formatter.Deserialize(stream) as PlayerData;
        }
    }
}