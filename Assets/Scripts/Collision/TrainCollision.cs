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

    private string GetTag(GameObject obj){
        Component[] collisions;

        collisions = obj.GetComponents(typeof(CollisionSignal));

        if(collisions.Length > 0){
            return obj.GetComponent<CollisionSignal>().collision_tag;
        }else{
            return "";
        }
    }

    private void Collision(){
        GameObject[] data = EventManager.GetData("Collision") as GameObject[];
        if(data[1] == trainhead){ //other object hit trainhead
            string tag = GetTag(data[0]);
            if(tag == "rock"){
                controller.Crash();
            }
            if(tag == "coin"){
                //Destroy(data[0]);
                //data[0].SetActive(false);
                data[0].GetComponent<Coin>().ActivateCoin();
                EventManager.EmitEvent("CoinCollected");
            }
            if(tag == "cow"){
                data[0].GetComponent<Cow>().Honk();
            }
        }
    }
}
