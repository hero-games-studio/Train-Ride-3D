using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TwoWayJunction : AbstractTrack
{
    // Start is called before the first frame update
    public int _picked_dir;

    private bool locked = false;

    private bool active = false;
    public PathSpline leftpath;
    public PathSpline rightpath;

    void Start()
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

        rightpath = new PathSpline();
        Transform rightpathobj = transform.Find("RightPath");
        for (int i = 0; i < rightpathobj.childCount; i++)
        {   
            Transform child = rightpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            rightpath.AddNode(newnode);
        }



        update_visual();
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    override public bool usable_junction(){
        return true;
    }

    override public PathSpline GetPath(){
        if(_picked_dir<=0){
            return leftpath;
        } else {
            return rightpath;
        }
    }

    override public PathSpline GetPathDir(int dir){
        
        if(dir<=0){
            return leftpath;
        } else {
            return rightpath;
        }
    }

    private void update_visual(){
        if(locked){
            //transform.Find("LeftVisual").gameObject.SetActive(true);
            //transform.Find("RightVisual").gameObject.SetActive(true);
            transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            return;
        }
        Vector4 active_color;
        if(active){
            active_color = AbstractTrack.jc_active;
        } else {
            active_color = AbstractTrack.jc_queued;
        }

        if(_picked_dir == -1){
            transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("LeftVisual").gameObject.SetActive(true);
            transform.Find("RightVisual").gameObject.SetActive(false);
            //transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
        } else {
            transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("LeftVisual").gameObject.SetActive(false);
            transform.Find("RightVisual").gameObject.SetActive(true);
            //transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
        }
    }

    private void update_direction(int dir){
        if(locked){
            return;
        }
        if(dir<=0){
            _picked_dir = -1;
        }else{
            _picked_dir = 1;
        }
        update_visual();
    }
    override public void toggle_direction(){
        if(_picked_dir == -1){
            update_direction(1);
        }else if(_picked_dir == 1){
            update_direction(-1);
        }else{
            update_direction(-1);
        }
    }

    override public void TagNextJunction(){
        active = true;
        update_visual();
    }
    override public void lock_track(){
        if(usable_junction()){
            TapDetection.UpdateNextJunction(this);
        }
        locked = true;
        active = false;
        update_visual();
    }

    override public int GetOffset(){
        return _picked_dir;
    }

    override public float get_miny(){
        return transform.Find("LeftPath").Find("p0").position.z;
    }

    override public float get_maxy(){
        return transform.Find("LeftPath").Find("p10").position.z;
    }

 

    override public Vector3 GetCenter(){
        return this.gameObject.transform.position + new Vector3(0,0,2.5f);
    }
}
