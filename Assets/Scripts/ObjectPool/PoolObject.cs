using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PoolObject : MonoBehaviour
{
    public string typeTag;
    public int amountToPool;
}
