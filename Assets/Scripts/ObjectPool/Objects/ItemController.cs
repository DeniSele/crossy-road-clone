using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : PoolObject
{
    [SerializeField]
    private List<GameObject> models;

    private void OnEnable()
    {
        if(models.Count == 0)
        {
            return;
        }

        models[Random.Range(0, models.Count)].SetActive(true);
    }

    private void OnDisable()
    {
        for(int i = 0; i < models.Count; i++)
        {
            models[i].SetActive(false);
        }
    }
}
