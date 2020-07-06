using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : PoolObject
{
    public int minSpawnAmount;
    public int maxSpawnAmount;

    [Header("Items generator")]

    [SerializeField]
    private bool moveableDetails = false;

    [SerializeField]
    private List<MoveableObjectSpawner> spawners;

    [SerializeField]
    private List<ItemController> itemsToSpawn;


    private List<ItemController> curItems = new List<ItemController>(5);
    private float spawnChance = 0.1f;
    private static int PlatformWidth = 9;

    private void OnEnable()
    {
        if (moveableDetails)
        {
            SpawnMoveableDetails();
        }
        else
        {
            SpawnStaticDetails();
        }
    }

    private void OnDisable()
    {
        RemoveAllDetails();
    }

    private void SpawnMoveableDetails()
    {
        if (spawners.Count == 0)
            return;

        int randomItem = Random.Range(0, spawners.Count);
        spawners[randomItem].StartSpawn();
    }

    private void SpawnStaticDetails()
    {
        if (itemsToSpawn.Count == 0)
            return;

        int randomItem;
        float randomValue;
        Vector3 firstSpawnPlace = transform.position - new Vector3((int)PlatformWidth/2, 0, 0);

        for (int i = 0; i < PlatformWidth; i++)
        {
            randomValue = Random.Range(0, 1f);
            if(randomValue < spawnChance)
            {
                randomItem = Random.Range(0, itemsToSpawn.Count);
                var item = (ItemController)ObjectPool.Instance.Get(itemsToSpawn[randomItem].typeTag);
                item.transform.position = firstSpawnPlace + new Vector3(i, 1, 0);
                item.gameObject.SetActive(true);
                curItems.Add(item);
            }
        }
    }

    private void RemoveAllDetails()
    {
        foreach(ItemController item in curItems)
        {
            item.gameObject.SetActive(false);
        }
        curItems.Clear();

        foreach (MoveableObjectSpawner item in spawners)
        {
            item.StopSpawn();
        }
    }
}
