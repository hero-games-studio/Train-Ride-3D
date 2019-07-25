using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(train_head == null){
            train_head = GameObject.FindGameObjectWithTag("TrainHead");
            offset = -transform.position + train_head.transform.position;
            return;
        }
        transform.position = train_head.transform.position - offset;
    }
}
