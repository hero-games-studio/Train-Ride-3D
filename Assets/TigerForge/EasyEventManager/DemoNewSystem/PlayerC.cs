using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class PlayerC : MonoBehaviour
{
    void Start()
    {
        EventManager.StartListening("I_AM_PLAYER", gameObject, MyListenerCallBack);
    }

    void MyListenerCallBack()
    {
        Debug.Log("I am: PlayerC.");
        var sender = EventManager.GetSender("I_AM_PLAYER");
        if (sender != null)
        {
            GameObject go = (GameObject)sender;
            Debug.Log("...and I know that the guy who called me is: " + go.name);
        }
    }
}
