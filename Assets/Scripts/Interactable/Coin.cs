using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool activated = false;
    private Vector3 dir;
    private float speed = 10f;
    private Vector3 acc;
    void Start()
    {
        
    }

    // Update is called once per frame
    GameObject train_head;
    Vector3 offset = new Vector3(0, -2f,-2f);
    void Update()
    {
        if(!activated){
            return;
        }
        if(train_head == null){
            train_head = Global.Instance.train_head;
        }
        if(train_head == null){
            return;
        }

        transform.Find("Cylinder").Find("obj").position += dir*Time.deltaTime*speed;
        transform.Find("Cylinder").Find("obj").localScale *= 0.98f;
        transform.Find("Cylinder").Find("obj").Rotate(Time.deltaTime*200,Mathf.Sign(dir.x)*Time.deltaTime,0);

        acc = ((train_head.transform.position + offset) - transform.Find("Cylinder").Find("obj").position).normalized;

        dir = (dir + acc*Time.deltaTime*15).normalized;

        if(( (train_head.transform.position + offset) - transform.Find("Cylinder").Find("obj").position).magnitude< 0.5f){
            activated = false;
            gameObject.SetActive(false);
        }

    }

    public static int side_dir = 15;

    public void ActivateCoin(){
        train_head = Global.Instance.train_head;
        if(train_head == null){
            return;
        }
        
        transform.Find("Cylinder").Find("obj").Find("particle").GetComponent<ParticleSystem>().Play();
        int max_sound = transform.Find("audio").childCount;
        int random = ((int) (transform.position.x*transform.position.z))%max_sound;

        transform.Find("audio").GetChild(random).GetComponent<AudioSource>().Play();

        gameObject.GetComponent<Animator>().SetBool("Bob",false);
        dir = new Vector3(side_dir,15,-5f).normalized;
        side_dir = -side_dir;
        activated = true;

    }

    public void Reset(){
        gameObject.SetActive(true);
        activated = false;
        transform.Find("Cylinder").Find("obj").position = transform.position + new Vector3(0,0.7f,0);
        gameObject.GetComponent<Animator>().SetBool("Bob",true);
        transform.Find("Cylinder").Find("obj").Find("particle").GetComponent<ParticleSystem>().Stop();

        transform.Find("Cylinder").Find("obj").localScale = new Vector3(0.5f,0.025f,0.5f);
        transform.Find("Cylinder").Find("obj").eulerAngles = new Vector3(90,0,0);
    }
}
