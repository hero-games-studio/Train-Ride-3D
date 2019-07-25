using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TwoWayJunction : AbstractTrack
{
    // Start is called before the first frame update
    private int _picked_dir;
    public PathSpline leftpath;
    public PathSpline rightpath;
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

        rightpath = new PathSpline();
        Transform rightpathobj = transform.Find("RightPath");
        for (int i = 0; i < rightpathobj.childCount; i++)
        {   
            Transform child = rightpathobj.Find("p"+i);
            PathNode newnode = new PathNode(child.position.x,child.position.z,child.position.y);
            rightpath.AddNode(newnode);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        bool picked = false;
        if(_picked_dir == -1){
            picked = true;
        }
        transform.Find("LeftVisual").gameObject.SetActive(picked);
        transform.Find("RightVisual").gameObject.SetActive(!picked);
    }

    private void update_direction(int dir){
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

    override public int GetOffset(){
        return _picked_dir;
    }
}
