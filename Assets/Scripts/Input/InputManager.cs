using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private float minSwipeDistance = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            fingerUpPosition = Input.mousePosition;
            fingerDownPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            fingerUpPosition = Input.mousePosition;
            DetectSwipe();
        }
        #endif

        foreach (Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if(touch.phase == TouchPhase.Ended)
            {
                fingerUpPosition = touch.position;
                DetectSwipe();        
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheck())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerUpPosition.y - fingerDownPosition.y > 0 ? Direction.Up : Direction.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerUpPosition.x - fingerDownPosition.x > 0 ? Direction.Right : Direction.Left;
                SendSwipe(direction);
            }

            fingerDownPosition = fingerUpPosition;
        }
        else
        {
            SendSwipe(Direction.Up);
        }
    }

    private bool SwipeDistanceCheck()
    {
        return VerticalMovementDistance() > minSwipeDistance || HorizontalMovementDistance() > minSwipeDistance;
    }

    private bool IsVerticalSwipe() => VerticalMovementDistance() > HorizontalMovementDistance();

    private float VerticalMovementDistance() => Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);

    private float HorizontalMovementDistance() => Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);

    private void SendSwipe(Direction direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        OnSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public Direction Direction;
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left
}