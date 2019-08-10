using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TwoJoin : AbstractTrack
{
    // Start is called before the first frame update
    public PathSpline leftpath;
    public PathSpline rightpath;

    override public void Init()
    {
        CalculateNextTrack();
        //Generate paths from children.

        RemakePath();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void RemakePath(){
        leftpath = new PathSpline();
        Transform leftpathobj = transform.Find("LeftPath");
        for (int i = leftpathobj.childCount-1; i > -1; i--)
        {   
            Transform child = leftpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            leftpath.AddNode(newnode);
        }


        rightpath = new PathSpline();
        Transform rightpathobj = transform.Find("RightPath");
        for (int i = rightpathobj.childCount-1; i > -1; i--)
        {   
            Transform child = rightpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            rightpath.AddNode(newnode);
        }
    }

  

    override public PathSpline GetPath(){
        GameObject last_obj = Global.Instance.last_inspected_track.gameObject;

        //print("TwoJoin: last_track name: "+last_obj.name);
        
        AbstractTrack last_track = last_obj.GetComponent<AbstractTrack>();
        if(last_track == null){
            return rightpath;
        }


        Vector3 twojoin_startpoint = GetStartPoint();


        if(twojoin_startpoint.x - transform.position.x < -1f){
            return leftpath;
        }else if(twojoin_startpoint.x - transform.position.x > 1f){
            return rightpath;
        }else{
            if(last_track.usable_junction()){
                if(last_track.GetOffset() <0){
                    return leftpath;
                }else{
                    return rightpath;
                }
            }
        }
        return rightpath;
    }

    override public PathSpline GetPathDir(int dir){
        return GetPath();
    }

    override public void toggle_direction(){
    }

    override public void lock_track(){

    }

    override public int GetOffset(){
        return 0;
    }

    override public float get_miny(){
        return transform.Find("LeftPath").Find("p10").position.z;
    }

    override public float get_maxy(){
        return transform.Find("LeftPath").Find("p0").position.z;
    }

 

    override public Vector3 GetCenter(){
        return this.gameObject.transform.position + new Vector3(0,0,2.5f);
    }

    override public Vector3 GetStartPoint(){
        GameObject last_obj = Global.Instance.last_inspected_track.gameObject;
        Vector3 last_pos = last_obj.GetComponent<AbstractTrack>().GetEndPoint();
        Vector3 picked = start_of_track[0].transform.position;

        if((start_of_track[1].transform.position - last_pos).magnitude < (picked - last_pos).magnitude ){
            picked = start_of_track[1].transform.position;
        }

        return picked;
    }
}
