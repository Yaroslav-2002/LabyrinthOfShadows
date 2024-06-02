using Entities;
using Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TrapManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private float checkInterval = 10.0f;
    [SerializeField] private float activeDistance = 50f;

    [SerializeField] private GameObject trap;

    [Inject] private PlayerGo player;
    private Transform playerTransform;
    private List<GameObject> entities = new();

    private void Start()
    {
        playerTransform = player.View.transform;
        StartCoroutine(ManageEntities());
    }

    public void SpawnTrap(Vector3 position)
    {
        entities.Add(Instantiate(trap, position, Quaternion.identity, transform));
    }

    private IEnumerator ManageEntities()
    {
        while (true)
        {
            foreach (GameObject entity in entities)
            {
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

            yield return new WaitForSeconds(checkInterval);
        }
    }

    public void LoadData(GameData data)
    {
        if(data.trapDatas != null) { 
            foreach(var trapData in data.trapDatas)
            {
                SpawnTrap(new Vector3(trapData.x, trapData.y, 0));
            }
        }
    }

    public void SaveData(GameData data)
    {
        List<TrapData> trapDatas = new List<TrapData>();

        foreach (GameObject trap in entities)
        {
            if (trap != null) // Check in case of destroyed or uninitialized objects
            {
                trapDatas.Add(new TrapData (trap.transform.position));
            }
        }

        data.trapDatas = trapDatas;
    }
}
