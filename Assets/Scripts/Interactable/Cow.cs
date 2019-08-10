using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : HonkAffectee
{
    
    override public void Init(){
        initial_position = transform.localPosition;
        initial_rotation = transform.localRotation;
    }

    private Vector3 initial_position;
    private Quaternion initial_rotation;
    private float speed = 15f;
    private bool active = false;
    private Vector3 dir;
    override public void Honk(){
        //Destroy(gameObject);
        if(active){
            return;
        }
        active = true;
        gameObject.GetComponent<Animator>().SetTrigger("Walk");
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void Reset(){
        active = false;
        transform.localPosition = initial_position;
        transform.localRotation = initial_rotation;
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
            transform.localPosition + Quaternion.AngleAxis(90, Vector3.up) * dir
        );
        transform.localPosition += dir*Time.deltaTime*speed;
    }
}
