using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeLogger : MonoBehaviour
{
    private void Awake()
    {
        InputManager.OnSwipe += InputManager_OnSwipe;
    }

    private void InputManager_OnSwipe(SwipeData data)
    {
        Debug.Log("Swipe in Direction: " + data.Direction);
    }
}
