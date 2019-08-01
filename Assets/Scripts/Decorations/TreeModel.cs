using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeModel : MonoBehaviour
{
    // Start is called before the first frame update
    Transform lv;
    Vector3 lv_in;
    Transform lv1;
    Vector3 lv1_in;

    Transform lv2;
    Vector3 lv2_in;

    float counter = 0;
    void Start()
    {
        lv = transform.Find("lv");
        lv_in = lv.transform.position;
        lv1 = transform.Find("lv1");
        lv1_in = lv1.transform.position;
        lv2 = transform.Find("lv2");
        lv2_in = lv2.transform.position;
        counter += new System.Random().Next(3000)/1000;
    }

    // Update is called once per frame
    float diff;
    void Update()
    {
        if(lv == null || lv1 == null || lv2 == null || Global.Instance.train_head == null){
            return;
        }
        diff = (transform.position - Global.Instance.train_head.transform.position).magnitude;

        counter += Global.Instance.train_speed*Time.deltaTime*2/diff;
        lv.transform.position = lv_in + new Vector3(Mathf.Sin(counter),Mathf.Sin(counter)/2,Mathf.Sin(counter))/7;
        lv1.transform.position = lv1_in + new Vector3(Mathf.Sin(counter+1),Mathf.Sin(counter+1)/2,Mathf.Sin(counter+1))/7;
        lv2.transform.position = lv2_in + new Vector3(Mathf.Sin(counter+2),Mathf.Sin(counter+2)/2,Mathf.Sin(counter+2))/10;
    }
}
