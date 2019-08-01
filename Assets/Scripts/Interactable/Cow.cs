using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : HonkAffectee
{
    
    override public void Init(){

    }

    private float speed = 15f;
    private bool active = false;
    private Vector3 dir;
    override public void Honk(){
        //Destroy(gameObject);
        active = true;
        gameObject.GetComponent<Animator>().SetTrigger("Walk");
    }

    void Update() {
        if(!active)
            return;

        dir = -(Global.Instance.train_head.transform.position - transform.position).normalized;
        dir.z /= 10;
        dir.y = 0;
        if(Mathf.Abs(dir.x) < 0.01f){
            dir.x = new System.Random().Next(0,2)*2 - 1;
        }
        transform.LookAt(
            transform.position + Quaternion.AngleAxis(90, Vector3.up) * dir
        );
        transform.position += dir*Time.deltaTime*speed;
    }
}
