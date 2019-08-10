﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public abstract class AbstractTrack : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected List<GameObject> start_of_track;
    [SerializeField] protected List<GameObject> end_of_track;
    void Start() {
        EventManager.StartListening("Crashed",lock_track);
        Init();
    }

    virtual public void Init(){

    }
    protected AbstractTrack next_track;
    abstract public PathSpline GetPath();

    abstract public PathSpline GetPathDir(int dir);

    abstract public int GetOffset();

    abstract public void toggle_direction();

    abstract public void lock_track();

    virtual public void unlock_track(){

    }

    abstract public float get_miny();
    abstract public float get_maxy();

    protected static Vector4 jc_queued = new Vector4(0.47f,0.37f,0.81f,1)*255;
    protected static Vector4 jc_passive = new Vector4(107,108,100,255);
    protected static Vector4 jc_active = new Vector4(255,4,4,255);

    virtual public bool usable_junction(){
        return false;
    }


    virtual public float GetYOffset(){
        return 6f;
    }

    virtual public void CalculateNextTrack(){
        next_track = Global.Instance.tracks_object.GetNextTrack(this.gameObject);
    }

    virtual public AbstractTrack GetNextTrack(){
        CalculateNextTrack();
        return next_track;
    }

    virtual public Vector3 GetCenter(){
        return this.gameObject.transform.position + new Vector3(0,0,2.5f);
    }

    virtual public void TagNextJunction(){

    }

    abstract public void RemakePath();

    virtual public void ResetTrack(){
        RemakePath();
        unlock_track();
    }

    public List<Vector3> getStartPoints(){
        List<Vector3> new_list = new List<Vector3>();
        foreach (GameObject item in start_of_track)
        {
            new_list.Add(item.transform.position);
        }
        return new_list;
    }

    public List<Vector3> getEndPoints(){
        List<Vector3> new_list = new List<Vector3>();
        foreach (GameObject item in end_of_track)
        {
            new_list.Add(item.transform.position);
        }
        return new_list;
    }

    virtual public Vector3 GetEndPoint(){
        return end_of_track[0].transform.position;
    }

    virtual public Vector3 GetStartPoint(){
        return start_of_track[0].transform.position;
    }
}
