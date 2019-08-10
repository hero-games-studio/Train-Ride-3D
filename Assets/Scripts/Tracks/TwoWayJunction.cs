using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;


public class TwoWayJunction : AbstractTrack
{
    // Start is called before the first frame update
    private int _picked_dir;

    private bool locked = false;

    public bool debug = false;

    private bool active = false;
    public PathSpline leftpath;
    public PathSpline rightpath;

    private SkinnedMeshRenderer mesh_renderer;

    
    override public void Init()
    {
        CalculateNextTrack();
        //Generate paths from children.
        RemakePath();

        mesh_renderer = transform.Find("VisualDir").gameObject.GetComponent<SkinnedMeshRenderer>();
        System.Random rand = new System.Random();
        _picked_dir = rand.Next(0,2)*2 - 1;
        update_visual();

        EventManager.StartListening("SwipeRight",swiperight);
        EventManager.StartListening("SwipeLeft",swipeleft);
    }

    override public void RemakePath(){
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

    public void swiperight(){
        if(usable_junction() && active){
            update_direction(1);
        }
    }

    public void swipeleft(){
        if(usable_junction() && active){
            update_direction(-1);
        }
    }

    private void update_visual(){
        if(locked){
            //transform.Find("LeftVisual").gameObject.SetActive(true);
            //transform.Find("RightVisual").gameObject.SetActive(true);
            /*
            transform.Find("LeftVisual").Find("LeftVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").Find("RightVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            */
            transform.Find("VisualDir").gameObject.GetComponent<SkinnedMeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
            return;
        }
        Vector4 active_color;
        if(active){
            active_color = AbstractTrack.jc_active;
        } else {
            active_color = AbstractTrack.jc_queued;
        }
        transform.Find("VisualDir").gameObject.GetComponent<SkinnedMeshRenderer>().material.SetVector("_Color",active_color/255);
        if(_picked_dir == -1){
            transform.Find("VisualDir").gameObject.GetComponent<Animator>().SetBool("right",false);
            /*
            transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("LeftVisual").Find("LeftVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("LeftVisual").gameObject.SetActive(true);
            transform.Find("RightVisual").gameObject.SetActive(false);
            */
            //transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
        } else {
            transform.Find("VisualDir").gameObject.GetComponent<Animator>().SetBool("right",true);
            /*
            transform.Find("RightVisual").Find("RightVisualC").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("RightVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",active_color/255);
            transform.Find("LeftVisual").gameObject.SetActive(false);
            transform.Find("RightVisual").gameObject.SetActive(true);
            */
            //transform.Find("LeftVisual").gameObject.GetComponent<MeshRenderer>().material.SetVector("_Color",AbstractTrack.jc_passive/255);
        }
    }

    private void update_direction(int dir){
        if(locked){
            return;
        }
        gameObject.GetComponent<AudioSource>().Play();
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
            GameHandler.UpdateNextJunction(this);
        }
        locked = true;
        active = false;
        update_visual();
    }

    override public void unlock_track(){
        locked = false;
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
