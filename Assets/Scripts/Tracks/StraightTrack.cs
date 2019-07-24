using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class StraightTrack : AbstractTrack
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public PathCreator GetPath(){
        return transform.Find("Path").GetComponent<PathCreator>();
    }

    override public PathCreator GetPathDir(int dir){
        return transform.Find("Path").GetComponent<PathCreator>();
    }

    
    override public void toggle_direction(){
    }
}
