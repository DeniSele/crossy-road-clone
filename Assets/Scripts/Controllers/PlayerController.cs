using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform targetModel;

    [Header("Jump animation")]

    [SerializeField] private float animationTime = 0.32f;

    [SerializeField] private float topJumpPower = 1f;

    [SerializeField] private float topJumpScale = 1.05f;

    private Vector3 targetPosition;
    private Vector3 startingPosition;
    private bool isDead = false;

    private const string CarObjectTag = "Car";
    private const string LogObjectTag = "Log";
    private const string WaterObjectTag = "Water";

    public static event Action<Vector3> OnJump = delegate { };
    public static event Action<Vector3> OnLog = delegate { };

    private void Awake()
    {
        InputManager.OnSwipe += Jump;
        DeathZone.OnDead += OnDead;
        UIController.ResetAll += Reset;

        startingPosition = transform.position;
        targetPosition = startingPosition;
    }

    private void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (transform.parent != null)
            OnLog(transform.position);
    }

    public void Reset()
    {
        transform.DOKill();
        transform.position = startingPosition;
        transform.rotation = Quaternion.identity;
        targetPosition = startingPosition;
        targetModel.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;

        isDead = false;
    }

    private void OnDead(string zoneTag)
    {
        isDead = true;
        transform.parent = null;
        targetModel.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    private void Jump(SwipeData swipeData)
    {
        if (isDead)
        {
            return;
        }

        if (transform.parent != null)
        {
            transform.SetParent(null);
            targetPosition = new Vector3(Convert.ToInt32(transform.position.x), Convert.ToInt32(transform.position.y), Convert.ToInt32(transform.position.z));
        }

        targetModel.DOLocalRotate(Vector3.up * 90 * (int)swipeData.Direction, animationTime, RotateMode.Fast);
        if ((int)swipeData.Direction % 2 == 0)
        {
            Vector3 targetVector = new Vector3(0, 0, 1 - (int)swipeData.Direction);
            if(CheckObstacle(targetVector))
            {
                return;
            }

            targetPosition += targetVector;
            transform.DOMove(targetPosition, animationTime, false).OnComplete(CheckUnderPlayer);
        }
        else
        {
            Vector3 targetVector = new Vector3(2 - (int)swipeData.Direction, 0, 0);
            if (CheckObstacle(targetVector))
            {
                return;
            }

            targetPosition += targetVector;
            transform.DOMove(targetPosition, animationTime, false).OnComplete(CheckUnderPlayer); ;
        }

        targetModel.DOScale(Vector3.one * topJumpScale, animationTime/2f).OnComplete(() => targetModel.DOScale(Vector3.one, animationTime/2));
        targetModel.DOLocalJump(Vector3.zero, topJumpPower, 1, animationTime, false);

        OnJump(targetPosition);
    }

    private void CheckUnderPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            if(hit.transform.gameObject.CompareTag(LogObjectTag))
            {
                transform.SetParent(hit.transform);
            }
            else
            {
                transform.SetParent(null);
            }

            if (hit.transform.gameObject.CompareTag(WaterObjectTag))
            {
                DeathZone.InvokeDeadByWaterEvent();
            }
        }
    }

    private bool CheckObstacle(Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 1f))
        {
            if(hit.transform.gameObject.CompareTag(CarObjectTag))
            {
                return false;
            }
            return true;
        }
        return false;
    }
}
