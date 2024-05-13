using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class RoomSerializer
{
    private static string SavePath = Path.Combine(Application.persistentDataPath, $"room_.save");

    public static void Serialize(RoomData roomData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(SavePath, FileMode.Create))
        {
            formatter.Serialize(stream, roomData);
        }
    }

    public static RoomData Deserialize()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogError($"Save file not found at {SavePath}.");
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(SavePath, FileMode.Open))
        {
            return formatter.Deserialize(stream) as RoomData;
        }
    }
}