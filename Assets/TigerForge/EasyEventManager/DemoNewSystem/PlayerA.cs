using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class PlayerA : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("I_AM_PLAYER_A", MyListenerCallBack);
    }

    void MyListenerCallBack()
    {
        Debug.Log("I am: PlayerA - My age is: " + EventManager.GetInt("I_AM_PLAYER_A"));
    }
}
