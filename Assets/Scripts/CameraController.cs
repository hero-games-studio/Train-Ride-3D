using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject train_head;
    private Vector3 offset;
    void Start()
    {
        //determine relative position
        offset = -transform.position + train_head.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = train_head.transform.position + offset;
    }
}
