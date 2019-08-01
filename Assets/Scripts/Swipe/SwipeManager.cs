
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
        Debug.Log("DoubleTap");
    }

    void OnTap()
    {
        EventManager.EmitEvent("Tap");
        Debug.Log("Tap");
    }

    void OnSwipeUp()
    {
        EventManager.EmitEvent("SwipeUp");
        Debug.Log("SwipeUp");
    }

    void OnSwipeDown()
    {
        EventManager.EmitEvent("SwipeDown");
        Debug.Log("SwipeDown");
    }

    void OnSwipeLeft()
    {
        EventManager.EmitEvent("SwipeLeft");
        Debug.Log("SwipeLeft");
    }

    void OnSwipeRight()
    {
        EventManager.EmitEvent("SwipeRight");
        Debug.Log("SwipeRight");
    }
}