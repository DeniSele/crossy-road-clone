using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private float maxDistanceFromPlayer = 3f;
    private float playerMaxZPosition = 0;

    private Vector3 cameraOffset = Vector3.zero;
    private Vector3 target = Vector3.zero;

    private void Awake()
    {
        cameraOffset = transform.position;
        target = cameraOffset;

        PlayerController.OnJump += ChangeTargetPosition;
        PlayerController.OnLog += FollowWhileOnLog;

        DeathZone.OnDead += OnDead;
        UIController.OnGameStart += SlowMoveLater;
        UIController.ResetAll += Reset;
    }

    public void Reset()
    {
        transform.DOKill();
        transform.position = cameraOffset;
        playerMaxZPosition = 0;
    }

    private void OnDead(string zoneTag)
    {
        transform.DOKill();
    }

    private void FollowWhileOnLog(Vector3 targetPosition)
    {
        transform.DOKill();
        target = new Vector3(targetPosition.x, targetPosition.y, playerMaxZPosition - 5);
        target += cameraOffset;
        transform.DOBlendableMoveBy(target - transform.position, 1f).OnComplete(SlowMoveLater);
    }

    private void ChangeTargetPosition(Vector3 targetPosition)
    {
        if (transform.position.z - cameraOffset.z > playerMaxZPosition - 5)
        {
            playerMaxZPosition = transform.position.z - cameraOffset.z + 5;
        }

        if (targetPosition.z > playerMaxZPosition)
        {
            playerMaxZPosition = targetPosition.z;
        }

        target = new Vector3(targetPosition.x, targetPosition.y, playerMaxZPosition - 5);
        target += cameraOffset;
        transform.DOBlendableMoveBy(target - transform.position, 1f);
    }

    private void SlowMoveLater()
    {
        transform.DOBlendableMoveBy(new Vector3(0, 0, maxDistanceFromPlayer), 8f).SetEase(Ease.Linear).OnComplete(SlowMoveLater);
    }
}
