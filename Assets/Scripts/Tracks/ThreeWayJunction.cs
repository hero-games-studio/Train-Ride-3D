using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TigerForge;

public class ThreeWayJunction : AbstractTrack
{
    // Start is called before the first frame update
    private int _picked_dir = 1;

    private bool locked = false;
    private bool active = false;
    
    public PathSpline leftpath;
    public PathSpline middlepath;
    public PathSpline rightpath;

    override public void Init()
    {
        CalculateNextTrack();
        //Generate paths from children.
        leftpath = new PathSpline();
        Transform leftpathobj = transform.Find("LeftPath");
        for (int i = 0; i < leftpathobj.childCount; i++)
        {   
            Transform child = leftpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            leftpath.AddNode(newnode);
        }

        middlepath = new PathSpline();
        Transform middlepathobj = transform.Find("MiddlePath");
        for (int i = 0; i < middlepathobj.childCount; i++)
        {   
            Transform child = middlepathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            middlepath.AddNode(newnode);
        }


        rightpath = new PathSpline();
        Transform rightpathobj = transform.Find("RightPath");
        for (int i = 0; i < rightpathobj.childCount; i++)
        {   
            Transform child = rightpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            rightpath.AddNode(newnode);
        }
        System.Random rand = new System.Random();
        _picked_dir = rand.Next(0,3) - 1;
        update_visual();

        EventManager.StartListening("SwipeRight",swiperight);
        EventManager.StartListening("SwipeUp",swipeup);
        EventManager.StartListening("SwipeLeft",swipeleft);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public PathSpline GetPath(){
        if(_picked_dir<0){
            return leftpath;
        } else if(_picked_dir>0) {
            return rightpath;
        } else{
            return middlepath;
        }
    }

    override public PathSpline GetPathDir(int dir){
        
        if(dir<0){
            return leftpath;
        } else if (dir>0) {
            return rightpath;
        } else{
            return middlepath;
        }
    }

    private void update_visual(){
        if(locked){
            transform.Find("LeftVisual").Find("LeftVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("MiddleVisual").Find("MiddleVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").Find("RightVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("MiddleVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            return;
        }
        Vector4 active_color;
        if(active){
            active_color = AbstractTrack.jc_active;
        } else {
            active_color = AbstractTrack.jc_queued;
        }

        transform.Find("LeftVisual").gameObject.SetActive(_picked_dir == -1);
        transform.Find("MiddleVisual").gameObject.SetActive(_picked_dir == 0);
        transform.Find("RightVisual").gameObject.SetActive(_picked_dir == 1);

        if(_picked_dir == -1){
            transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("MiddleVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("LeftVisual").Find("LeftVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("MiddleVisual").Find("MiddleVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").Find("RightVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
        } else if(_picked_dir == 0){
            transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("MiddleVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("LeftVisual").Find("LeftVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("MiddleVisual").Find("MiddleVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("RightVisual").Find("RightVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
        } else {
            transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("MiddleVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("LeftVisual").Find("LeftVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("MiddleVisual").Find("MiddleVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").Find("RightVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
        }


    
        
    }

    private void update_direction(int dir){
        if(locked){
            return;
        }
        if(dir<0){
            _picked_dir = -1;
        }else if(dir>0){
            _picked_dir = 1;
        }else{
            _picked_dir = 0;
        }
        update_visual();
    }

    public void swiperight(){
        if(usable_junction() && active){
            update_direction(1);
        }
    }

    public void swipeup(){
        if(usable_junction() && active){
            update_direction(0);
        }
    }
    public void swipeleft(){
        if(usable_junction() && active){
            update_direction(-1);
        }
    }
    override public bool usable_junction(){
        return true;
    }
    override public void toggle_direction(){
        if(_picked_dir == -1){
            update_direction(0);
        }else if(_picked_dir == 0){
            update_direction(1);
        }else{
            update_direction(-1);
        }
    }

    override public void lock_track(){
        if(usable_junction()){
            TapDetection.UpdateNextJunction(this);
        }
        locked = true;
        update_visual();
    }

    override public void TagNextJunction(){
        active = true;
        update_visual();
    }

    override public int GetOffset(){
        return (_picked_dir);
    }

    override public float get_miny(){
        return transform.Find("MiddlePath").Find("p0").position.z;
    }

    override public float get_maxy(){
        return transform.Find("MiddlePath").Find("p1").position.z;
    }


    override public Vector3 GetCenter(){
        return this.gameObject.transform.position + new Vector3(0,0,2.5f);
    }
}
