using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public bool HasSave()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        return File.Exists(fullPath);
    }

    public async Task<GameData> LoadAsync()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("No data file found at: " + fullPath);
            return null;
        }

        return await Task.Run(() =>
        {
            try
            {
                using (StreamReader reader = new StreamReader(fullPath))
                {
                    string jsonData = reader.ReadToEnd();
                    GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);
                    Debug.Log("Data loaded successfully from " + fullPath);
                    return loadedData;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load data: " + e.Message);
                return null;
            }
        });
    }

    public async Task SaveAsync(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        await Task.Run(() =>
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    string jsonData = JsonUtility.ToJson(data);
                    writer.Write(jsonData);
                    Debug.Log("Data saved successfully to " + fullPath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save data: " + e.Message);
            }
        });
    }
}
