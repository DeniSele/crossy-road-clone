using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesLauncher : MonoBehaviour
{
    private ParticleSystem particles;

    private const string WaterZoneTag = "water";

    private void Awake()
    {
        DeathZone.OnDead += PlayEffect;
    }

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    private void PlayEffect(string zoneTag)
    {
        if (zoneTag == WaterZoneTag)
        {
            particles.Play();
        }
    }
}
