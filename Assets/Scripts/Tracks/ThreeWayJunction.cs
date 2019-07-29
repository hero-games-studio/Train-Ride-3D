using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeWayJunction : AbstractTrack
{
    // Start is called before the first frame update
    private int _picked_dir;

    private bool locked = false;
    
    private bool rotated = false;
    public PathSpline leftpath;
    public PathSpline middlepath;
    public PathSpline rightpath;

    public PathSpline leftpath_reverse;
    public PathSpline middlepath_reverse;
    public PathSpline rightpath_reverse;
    void Start()
    {
        //Generate paths from children.
        leftpath = new PathSpline();
        Transform leftpathobj = transform.Find("LeftPath");
        for (int i = 0; i < leftpathobj.childCount; i++)
        {   
            Transform child = leftpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            leftpath.AddNode(newnode);
        }

        leftpath_reverse = new PathSpline();
        for (int i = leftpathobj.childCount-1; i > -1; i--)
        {   
            Transform child = leftpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            leftpath_reverse.AddNode(newnode);
        }

        middlepath = new PathSpline();
        Transform middlepathobj = transform.Find("MiddlePath");
        for (int i = 0; i < middlepathobj.childCount; i++)
        {   
            Transform child = middlepathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            middlepath.AddNode(newnode);
        }

        middlepath_reverse = new PathSpline();
        for (int i = middlepathobj.childCount-1; i > -1; i--)
        {   
            Transform child = middlepathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            middlepath_reverse.AddNode(newnode);
        }

        rightpath = new PathSpline();
        Transform rightpathobj = transform.Find("RightPath");
        for (int i = 0; i < rightpathobj.childCount; i++)
        {   
            Transform child = rightpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            rightpath.AddNode(newnode);
        }

        rightpath_reverse = new PathSpline();
        for (int i = rightpathobj.childCount-1; i > -1; i--)
        {   
            Transform child = rightpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            rightpath_reverse.AddNode(newnode);
        }

        if(Mathf.RoundToInt(transform.eulerAngles.y) == 180){
            rotated = true;
            lock_track();
        }

        _picked_dir = 1;
        update_visual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public PathSpline GetPath(){
        if(rotated){
            GameObject tr_head = GameObject.FindGameObjectWithTag("TrainHead");
            if(tr_head.transform.position.x-transform.position.x < -0.1f){
                return rightpath_reverse;
            }else if(tr_head.transform.position.x-transform.position.x > 0.1f){
                return leftpath_reverse;
            }else{
                return middlepath_reverse;
            }
        }
        print("GET PATH DIR: "+_picked_dir);
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
            transform.Find("LeftVisual").gameObject.SetActive(false);
            transform.Find("MiddleVisual").gameObject.SetActive(false);
            transform.Find("RightVisual").gameObject.SetActive(false);
            return;
        }
    
        transform.Find("LeftVisual").gameObject.SetActive(_picked_dir == -1);
        transform.Find("MiddleVisual").gameObject.SetActive(_picked_dir == 0);
        transform.Find("RightVisual").gameObject.SetActive(_picked_dir == 1);
    }

    private void update_direction(int dir){
        print(dir);
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

    override public bool usable_junction(){
        if(rotated){
            return false;
        }
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

    override public int GetOffset(){
        return (_picked_dir);
    }

    override public float get_miny(){
        return transform.Find("MiddlePath").Find("p0").position.z;
    }

    override public float get_maxy(){
        return transform.Find("MiddlePath").Find("p1").position.z;
    }

    override public float GetYOffset(){
        if(rotated){
            return 2.5f;
        }
        return 7.5f;
    }
}
