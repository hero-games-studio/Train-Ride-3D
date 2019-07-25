using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class TwoWayJunction : AbstractTrack
{
    // Start is called before the first frame update
    private int _picked_dir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public PathCreator GetPath(){
        if(_picked_dir<=0){
            return transform.Find("PathLeft").GetComponent<PathCreator>();
        } else {
            return transform.Find("PathRight").GetComponent<PathCreator>();
        }
    }

    override public PathCreator GetPathDir(int dir){
        if(dir<=0){
            return transform.Find("PathLeft").GetComponent<PathCreator>();
        } else {
            return transform.Find("PathRight").GetComponent<PathCreator>();
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
