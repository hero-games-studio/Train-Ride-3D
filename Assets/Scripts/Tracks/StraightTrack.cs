using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightTrack : AbstractTrack
{
    // Start is called before the first frame update
    public int offset = 0;
    private  PathSpline path;
    private int count = 0;
    override public void Init()
    {
        CalculateNextTrack();
        RemakePath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void RemakePath(){
        path = new PathSpline();
        Transform pathobj = transform.Find("Path");
        for (int i = 0; i < pathobj.childCount; i++)
        {   
            count = i;
            Transform child = pathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            path.AddNode(newnode);
        }
    }
    override public PathSpline GetPath(){
        if(path == null){
            Debug.LogError("Path is null");
            Init();
            return path;
        }
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
        if(offset == 0){
            return 0;
        }else{
            return (int) Mathf.Sign(transform.localScale.x);
        }
    }

    override public float get_miny(){
        return transform.Find("Path").Find("p0").position.z;
    }

    override public float get_maxy(){
        return transform.Find("Path").Find("p"+count).position.z;
    }
}
