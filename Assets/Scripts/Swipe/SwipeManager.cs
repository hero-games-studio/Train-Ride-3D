
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TigerForge;

public class SwipeManager : MonoBehaviour
{

    void Update()
    {
        if (SwipeInput.Instance.DoubleTap)
            OnDoubleTap();
        if (SwipeInput.Instance.Tap)
            OnTap();
        if (SwipeInput.Instance.SwipeUp)
            OnSwipeUp();
        if (SwipeInput.Instance.SwipeDown)
            OnSwipeDown();
        if (SwipeInput.Instance.SwipeLeft)
            OnSwipeLeft();
        if (SwipeInput.Instance.SwipeRight)
            OnSwipeRight();
    }

    void OnDoubleTap()
    {
        EventManager.EmitEvent("DoubleTap");
    }

    void OnTap()
    {
        EventManager.EmitEvent("Tap");
    }

    void OnSwipeUp()
    {
        EventManager.EmitEvent("SwipeUp");
    }

    void OnSwipeDown()
    {
        EventManager.EmitEvent("SwipeDown");
    }

    void OnSwipeLeft()
    {
        EventManager.EmitEvent("SwipeLeft");
    }

    void OnSwipeRight()
    {
        EventManager.EmitEvent("SwipeRight");
    }
}