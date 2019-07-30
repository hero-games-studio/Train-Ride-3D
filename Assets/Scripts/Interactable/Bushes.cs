using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushes : Interactable
{
    // Start is called before the first frame update
    private int tap_count = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DeleteObj(string name){
        transform.Find(name).gameObject.SetActive(false);
    }

    override public void OnTap(){
        tap_count++;
        switch(tap_count){
            case 1:
                DeleteObj("Cube");
                break;
            case 2:
                DeleteObj("Cube (1)");
                break;
            case 3:
                Destroy(gameObject);
                break;
        }
        GetComponent<Animator>().Play("OnTap");
    }
}
