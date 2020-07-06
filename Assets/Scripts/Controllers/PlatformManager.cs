using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField]
    private List<PlatformController> platformTypes;

    [SerializeField]
    private int maxPlatformsCount = 20;

    [SerializeField]
    private float maxDistanceToPlayer = 20;

    public static PlatformManager Instance;

    private Vector3 currentSpawnPosition = Vector3.zero;
    private Queue<PlatformController> platforms = new Queue<PlatformController>();

    private void Awake()
    {
        Instance = this;
        PlayerController.OnJump += ShiftPlatform;
    }

    private void Start()
    {
        GenerateStart();
    }

    public void Reset()
    {
        while(platforms.Count > 0)
        {
            RemovePlatform();
        }

        currentSpawnPosition = Vector3.zero;
        GenerateStart();
    }

    private void GenerateStart()
    {
        SpawnPlatform(7, "grass");

        while (platforms.Count < maxPlatformsCount)
        {
            ChooseRandomPlatform(out string typeTag, out int amountToSpawn);
            SpawnPlatform(amountToSpawn, typeTag);
        }
    }

    private void ShiftPlatform(Vector3 targetPosition)
    {
        if (currentSpawnPosition.z - targetPosition.z < maxDistanceToPlayer)
        {
            RemovePlatform();

            if (platforms.Count > maxPlatformsCount)
            {
                return;
            }

            ChooseRandomPlatform(out string typeTag, out int amountToSpawn);
            SpawnPlatform(amountToSpawn, typeTag);
        }
    }

    private void SpawnPlatform(int count, string typeTag)
    {
        for (int i = 0; i < count; i++)
        {
            var platform = (PlatformController)ObjectPool.Instance.Get(typeTag);
            platform.transform.position = currentSpawnPosition;
            platform.gameObject.SetActive(true);
            platforms.Enqueue(platform);

            currentSpawnPosition += Vector3.forward;
        }
    }

    private void RemovePlatform()
    {
        if (platforms.Count > 0)
        {
            var platform = platforms.Dequeue();
            platform.gameObject.SetActive(false);
        }
    }

    private void ChooseRandomPlatform(out string typeTag, out int amountToSpawn)
    {
        var platform = platformTypes[Random.Range(0, platformTypes.Count)];
        typeTag = platform.typeTag;
        amountToSpawn = Random.Range(platform.minSpawnAmount, platform.maxSpawnAmount + 1);
    }
}