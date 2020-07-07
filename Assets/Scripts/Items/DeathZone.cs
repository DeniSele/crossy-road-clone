using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private string zoneTag = "default";
    private static bool isDead = false;

    private const string PlayerTag = "Player";
    private const string WaterZoneTag = "water";

    public static event Action<string> OnDead = delegate { };

    public static void InvokeDeadByWaterEvent()
    {
        isDead = true;
        OnDead(WaterZoneTag);
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
        if(!isDead && collision.gameObject.CompareTag(PlayerTag))
        {
            isDead = true;
            OnDead(zoneTag);
        }
    }
}
