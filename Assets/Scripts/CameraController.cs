using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TigerForge;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject train_head;
    private Vector3 offset;
    void Start()
    {
        //determine relative position
        train_head = GameObject.FindGameObjectWithTag("TrainHead");

        if(train_head != null){
            offset = -transform.position + train_head.transform.position;
        }else{
            offset = new Vector3(0,0,0);
        }
        EventManager.StartListening("Crashed",Crash);
    }

    // Update is called once per frame
    void Update()
    {
        if(train_head == null){
            train_head = GameObject.FindGameObjectWithTag("TrainHead");
            return;
        }
        gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_Follow = train_head.transform;
        gameObject.GetComponent<Cinemachine.CinemachineVirtualCamera>().m_LookAt = train_head.transform;
    }

    public void Crash(){
        gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }

    public void UpdateCamera(){
        if(train_head == null){
            train_head = GameObject.FindGameObjectWithTag("TrainHead");
            offset = -transform.position + train_head.transform.position;
            return;
        }
       
        //transform.position = new Vector3(train_head.transform.position.x,(train_head.transform.position - offset).y,(train_head.transform.position - offset).z);
        
    }
}
