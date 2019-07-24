using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class PlayerB : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("I_AM_PLAYER", gameObject, MyListenerCallBack);
    }

    void MyListenerCallBack()
    {
        Debug.Log("I am: PlayerB.");
    }
}
