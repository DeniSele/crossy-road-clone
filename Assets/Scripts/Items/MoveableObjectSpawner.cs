using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveableObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private List<ItemController> itemsToSpawn;

    [SerializeField]
    private float minSpawnTime = 4f;

    [SerializeField]
    private float maxSpawnTime = 7f;

    [SerializeField]
    private float pathLength = 19;

    private Queue<ItemController> activeItems = new Queue<ItemController>();

    private Vector3 spawnPosition;
    private Quaternion backturn = new Quaternion(0, 180, 0, 0);
    
    private float moveTime;
    private float nextSpawnTime;

    private bool isSpawning = false;

    private void FixedUpdate()
    {
        if(isSpawning && nextSpawnTime < Time.time)
        {
            ReloadTimer();
            SpawnItem();
        }
    }

    public void StartSpawn()
    {
        spawnPosition = transform.position + new Vector3(0, 1, 0);
        isSpawning = true;
        moveTime = Random.Range(6, 10f);
        nextSpawnTime = Time.time + Random.Range(0, 3f);
    }

    public void StopSpawn()
    {       
        isSpawning = false;
        while(activeItems.Count > 0)
        {
            DespawnItem();
        }
    }

    private void ReloadTimer() => nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);

    private void SpawnItem()
    {
        int randomItem = Random.Range(0, itemsToSpawn.Count);

        var item = (ItemController)ObjectPool.Instance.Get(itemsToSpawn[randomItem].typeTag);
        activeItems.Enqueue(item);

        item.gameObject.transform.position = spawnPosition;
        item.transform.rotation = pathLength < 0 ? backturn : Quaternion.identity;

        item.gameObject.SetActive(true);
        item.gameObject.transform.DOLocalMoveX(spawnPosition.x + pathLength, moveTime).SetEase(Ease.Linear).OnComplete(DespawnItem);
    }

    private void DespawnItem() {
        var item = activeItems.Dequeue();
        item.gameObject.transform.DOKill();
        item.gameObject.SetActive(false);
    }
}
