using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class Honk : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float start_time = 0;
    bool active = false;
    // Update is called once per frame
    void Update()
    {
        if(!active){
            return;
        }
        start_time += Time.deltaTime;

        transform.Find("Sphere").gameObject.GetComponent<MeshRenderer>().material.SetFloat("_SavedTime",start_time);
        if(start_time>0.45f){
            transform.Find("Sphere").gameObject.SetActive(false);
            active = false;
        }
    }

    override public void OnTap(){
        print("tapped");
        EventManager.SetData("Honk",gameObject);
        EventManager.EmitEvent("Honk");
        transform.Find("Sphere").gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().SetTrigger("Jump");
        start_time = 0;
        active = true;
    }
}
