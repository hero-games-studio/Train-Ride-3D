using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightTrack : AbstractTrack
{
    // Start is called before the first frame update
    private  PathSpline path;
    void Start()
    {
        path = new PathSpline();
        Transform pathobj = transform.Find("Path");
        for (int i = 0; i < pathobj.childCount; i++)
        {   
            Transform child = pathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            path.AddNode(newnode);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public PathSpline GetPath(){
        return path;
    }

    override public PathSpline GetPathDir(int dir){
        return path;
    }

    
    override public void toggle_direction(){
    }

    override public void lock_track(){
    }

    override public int GetOffset(){
        return 0;
    }

    override public float get_miny(){
        return transform.Find("Path").Find("p0").position.z;
    }

    override public float get_maxy(){
        return transform.Find("Path").Find("p1").position.z;
    }
}
