using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class Emitter : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("I'm the Emitter. I called PlayerA and I set his age to 24 yo.");
            EventManager.SetData("I_AM_PLAYER_A", 24);
            EventManager.EmitEvent("I_AM_PLAYER_A");
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("I'm the Emitter. I'll call the Players after 2 seconds, but only the Players with 'Player' tag will reply.");
            EventManager.EmitEvent("I_AM_PLAYER", "tag:Player", 2);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("I'm the Emitter. I'll call the Players placed in the 'Water' Layer. Moreover, I'll tell them who I am.");
            EventManager.EmitEvent("I_AM_PLAYER", "layer:4", 0, gameObject);
        }
    }
}
