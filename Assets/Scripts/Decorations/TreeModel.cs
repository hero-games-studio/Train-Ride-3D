    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeModel : MonoBehaviour
{
    
    public void Start()
    {

    }

    // Update is called once per frame
    
    void Update()
    {
       
    }

    public void SetTrunkColor(Color color){
        transform.Find("wood").GetComponent<MeshRenderer>().material.SetColor("_Color",color);
    }

    public void SetLeavesColor(Color color){
        transform.Find("leaves").GetComponent<MeshRenderer>().material.SetColor("_Color",color);
    }

    public void Reset(){
        float t = Mathf.Abs(transform.position.x*transform.position.z);

        transform.eulerAngles = new Vector3(t%15 / 5f,180 + (t%100-50),t%15 / 5f);
    }

    public void OnTriggerEnter(Collider other){
        if(other.gameObject.GetComponent<AbstractTrack>() != null){
            float sign = Mathf.Sign(other.gameObject.transform.position.x - transform.position.x);
            transform.position += new Vector3(-sign*15,0,0);
        }
    }
}
