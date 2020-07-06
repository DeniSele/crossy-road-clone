using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPicker : MonoBehaviour
{
    public static event Action<int> OnGoldPick = delegate { };

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OnGoldPick(1);
            gameObject.SetActive(false);
        }
    }
}
