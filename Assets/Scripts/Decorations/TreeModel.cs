    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeModel : MonoBehaviour
{
    
    public void Start()
    {

        float t = Mathf.Abs(transform.position.x*transform.position.z);

        transform.eulerAngles = new Vector3(t%15 / 5f,180 + (t%100-50),t%15 / 5f);

        foreach (GameObject item in Global.Instance.tracks_object.get_track_array)
        {
            if((transform.position - item.transform.position).magnitude < 3.75f){
                gameObject.SetActive(false);
            }
        }
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

    
}
