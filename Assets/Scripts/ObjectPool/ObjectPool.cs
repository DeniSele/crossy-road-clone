using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private List<PoolObject> itemsToPool = new List<PoolObject>();

    public static ObjectPool Instance { get; private set; }
    private List<PoolObject> objects = new List<PoolObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach(PoolObject poolObject in itemsToPool)
        {
            AddObjects(poolObject.amountToPool, poolObject.typeTag);
        }
    }

    public PoolObject Get(string typeTag)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if(objects[i].typeTag == typeTag && !objects[i].gameObject.activeSelf)
            {
                return objects[i];
            }
        }

        return AddObject(typeTag);
    }

    private PoolObject AddObject(string typeTag)
    {
        var poolObject = GetPoolObjectPrefab(typeTag);
        if (poolObject == null) {
            return null;
        }

        var currentPoolObject = GameObject.Instantiate(poolObject, transform);
        currentPoolObject.gameObject.SetActive(false);
        objects.Add(currentPoolObject);

        return currentPoolObject;
    }

    private void AddObjects(int count, string typeTag)
    {
        var poolObject = GetPoolObjectPrefab(typeTag);
        if (poolObject == null) {
            return;
        }
        
        for (int i = 0; i < count; i++)
        {
            var currentPoolObject = GameObject.Instantiate(poolObject, transform);
            currentPoolObject.gameObject.SetActive(false);
            objects.Add(currentPoolObject);
        }
    }

    private PoolObject GetPoolObjectPrefab(string typeTag)
    {
        foreach (PoolObject poolObject in itemsToPool)
        {
            if (poolObject.typeTag == typeTag)
            {
                return poolObject;
            }
        }
        return null;
    }
}
