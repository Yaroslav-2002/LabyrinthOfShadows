using UnityEngine;
using VContainer;
using System.Collections.Generic;
using Entities.Player;
using Generation;
using VContainer.Unity;
using Entities;
using System.Collections;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

public class EntitySpawnManager : MonoBehaviour, IDataPersistence
{
    [Inject] private IObjectResolver resolver;
    [Inject] private PlayerGo player;
    private Transform playerTransform;
    public List<GameObject> allEntities = new List<GameObject>();

    [SerializeField] private float checkInterval = 10.0f;
    [SerializeField] private float activeDistance = 50f;
    [SerializeField] private MapGenerationManager mapGenerationManager;
    [SerializeField] private GameObject skeleton;
    [SerializeField] private GameObject minotaur;

    private void Start()
    {
        playerTransform = player.View.transform;
        StartCoroutine(ManageEntities());
    }

    private IEnumerator ManageEntities()
    {
        while (true)
        {
            List<GameObject> entitiesToRemove = new List<GameObject>();

            foreach (GameObject entity in allEntities)
            {
                if(entity == null)
                {
                    entitiesToRemove.Add(entity);
                    continue;
                }
                float distance = Vector3.Distance(player.View.transform.position, entity.transform.position);
                bool shouldBeActive = distance <= activeDistance;

                if (shouldBeActive && !entity.activeSelf)
                {
                    entity.SetActive(true);
                }
                else if (!shouldBeActive && entity.activeSelf)
                {
                    entity.SetActive(false);
                }
            }

            foreach (GameObject entity in entitiesToRemove)
            {
                allEntities.Remove(entity);
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void OnEnable()
    {
        mapGenerationManager.OnMapGenerated += SpawnEntities;
    }

    private void OnDisable()
    {
        mapGenerationManager.OnMapGenerated -= SpawnEntities;
    }

    public void SpawnEntities(Dictionary<int, Room> rooms)
    {
        foreach (var room in rooms.Values)
        {
            foreach (var position in room.MobSpawnPositions)
            {
                GameObject skeletonInstance = resolver.Instantiate(skeleton, position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, transform);
                skeletonInstance.GetComponent<Enemy>().Initialize(null);
                allEntities.Add(skeletonInstance);
            }
        }
    }

    public void SpawnMinotaur(Vector3 position)
    {
        GameObject minotaurInstance = resolver.Instantiate(minotaur, position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity, transform);
        allEntities.Add(minotaurInstance);
    }

    public void SaveData(GameData data)
    {
        List<CreatureData> creatureDatas = new List<CreatureData>();

        foreach (GameObject enemyGo in allEntities)
        {
            Enemy enemy = enemyGo.GetComponent<Enemy>();
            if (enemy != null)
            {
                creatureDatas.Add(enemy.GetCreatureData());
            }
        }

        data.enemyDatas = creatureDatas;
    }

    public void LoadData(GameData data)
    {
        if (data.enemyDatas == null)
            return;

        foreach(var creatureData in data.enemyDatas) 
        {
            GameObject skeletonInstance = resolver.Instantiate(skeleton, new Vector3(creatureData.x, creatureData.y, 0), Quaternion.identity, transform);
            skeletonInstance.GetComponent<Enemy>().Initialize(creatureData);
            allEntities.Add(skeletonInstance);
        }
    }
}