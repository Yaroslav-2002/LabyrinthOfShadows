using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class DoorManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject doorPrefab;

    private List<GameObject> entities = new();

    public void SpawnDoor(Vector3 position, bool isOpened = false)
    {
        var door = Instantiate(doorPrefab, position, Quaternion.identity, transform);
        if (isOpened) door.GetComponent<DoorController>().Open();
        entities.Add(door);
    }

    public void LoadData(GameData data)
    {
        if (data.doorDatas != null)
        {
            foreach (var doorData in data.doorDatas)
            {
                SpawnDoor(new Vector3(doorData.x, doorData.y, 0), doorData.isOpened);
            }
        }
    }

    public void SaveData(GameData data)
    {
        List<DoorData> doorDatas = new List<DoorData>();

        foreach (GameObject door in entities)
        {
            if (door != null) // Check in case of destroyed or uninitialized objects
            {
                doorDatas.Add(new DoorData(door.transform.position, door.GetComponent<DoorController>().IsOpened));
            }
        }

        data.doorDatas = doorDatas;
    }
}
