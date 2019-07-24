using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class DemoEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {

        Debug.Log("Event registered!");

        //EventManager.StartListening("TEST", Test);

        EventManager.StartListening("TEST", gameObject, Test);

    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("Event emitted!");
            EventManager.SetData("TEST", "CIAO!!!");

            EventManager.EmitEvent("TEST", "tag:Players;name:MyPlayer");
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            Debug.Log("Event stopped!");
            EventManager.StopListening("TEST", Test);
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            Debug.Log("Event paused!");
            EventManager.PauseListening("TEST");
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            Debug.Log("Event restarted!");
            EventManager.RestartListening("TEST");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("Event stopped!");
            EventManager.StopAll();
        }
    }


    void Test()
    {
        Debug.Log("Event has been received! " + EventManager.GetBool("TEST") + " FROM: " + (string)EventManager.GetSender("TEST"));
    }

}
