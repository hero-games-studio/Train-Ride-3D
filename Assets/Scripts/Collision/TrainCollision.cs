using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class TrainCollision : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject trainhead;
    TrainController controller;
    void Start()
    {
        trainhead = GameObject.FindGameObjectWithTag("TrainHead");
        controller = GameObject.FindGameObjectWithTag("TrainController").GetComponent<TrainController>();
        EventManager.StartListening("Collision",Collision);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Collision(){
        GameObject[] data = EventManager.GetData("Collision") as GameObject[];
        if(data[1] == trainhead){ //other object hit trainhead
            if(data[0].GetComponent<CollisionSignal>().collision_tag == "rock"){
                controller.Crash();
            }
            if(data[0].GetComponent<CollisionSignal>().collision_tag == "coin"){
                Destroy(data[0]);
                EventManager.EmitEvent("CoinCollected");
            }
        }
    }
}
