using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private string zoneTag = "default";
    private static bool isDead = false;
    
    public static event Action<string> OnDead = delegate { };

    public static void InvokeDeadEvent()
    {
        isDead = true;
        OnDead("water");
    }

    private void Awake()
    {
        UIController.ResetAll += Reset;
    }

    private void Reset()
    {
        isDead = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(!isDead && collision.gameObject.tag == "Player")
        {
            isDead = true;
            OnDead(zoneTag);
        }
    }
}
