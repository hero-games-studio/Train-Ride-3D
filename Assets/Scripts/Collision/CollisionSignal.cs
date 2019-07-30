using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class CollisionSignal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string collision_tag = "";


    public void OnTriggerEnter(Collider other) {
        GameObject[] data = new GameObject[2];
        data[0] = gameObject;
        data[1] = other.gameObject;
        EventManager.SetData("Collision",data);
        EventManager.EmitEvent("Collision");
    }
}
