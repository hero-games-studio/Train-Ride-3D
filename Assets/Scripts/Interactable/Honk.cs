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

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void OnTap(){
        EventManager.SetData("Honk",gameObject);
        EventManager.EmitEvent("Honk");
    }
}
