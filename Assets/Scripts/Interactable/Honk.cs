using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.UI;

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
        Text txt = transform.Find("Canvas").Find("Text").gameObject.GetComponent<Text>();
        System.Random rand = new System.Random();
        int randnum = rand.Next(0,3);
        if(randnum == 0){
            txt.text = "CHOO";
        }
        if(randnum == 1){
            txt.text = "CHUFF";
        }
        if(randnum == 2){
            txt.text = "HONK";
        }
        gameObject.GetComponent<Animator>().SetTrigger("Jump");
        start_time = 0;
        active = true;
    }
}
