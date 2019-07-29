﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject starting_track;

    private List<GameObject> track_array = new List<GameObject>();

    public List<GameObject> get_track_array{
        get{
            return track_array;
        }
    }
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            track_array.Add(transform.GetChild(i).gameObject);
        }
        Global.Instance.tracks_object = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AbstractTrack GetNextTrack(){
        return starting_track.GetComponent<AbstractTrack>();
    }

    public AbstractTrack GetNextTrack(GameObject track){
        float x_offset =  track.GetComponent<AbstractTrack>().GetOffset();
        float z_offset = track.GetComponent<AbstractTrack>().GetYOffset();
        Vector3 ref_pos = track.GetComponent<AbstractTrack>().GetCenter() + new Vector3(x_offset*3,0,z_offset);

        GameObject closest = track;
        float dist = Mathf.Infinity;
        foreach (GameObject track_object in track_array)
        {
            float z_dist = -(track_object.transform.position.z - ref_pos.z);
            float this_dist = (track_object.GetComponent<AbstractTrack>().GetCenter() - ref_pos).magnitude;
            if(z_dist<0){
                //continue;
            }
            if(this_dist < dist){
                dist = this_dist;
                closest = track_object;
            }
        }

        return closest.GetComponent<AbstractTrack>();
    }

    public AbstractTrack GetNextTrack(Vector3 pos){
        GameObject track = gameObject;
        float dist2 = Mathf.Infinity;
        foreach (GameObject track_object in track_array)
        {
            float this_dist = (track_object.transform.position - pos).magnitude;
            if(this_dist < dist2){
                dist2 = this_dist;
                track = track_object;
            }
        }

        int offset =  track.GetComponent<AbstractTrack>().GetOffset();
        Vector3 ref_pos = track.transform.position + new Vector3(offset*3,0,4);
        GameObject closest = track;
        float dist = Mathf.Infinity;
        foreach (GameObject track_object in track_array)
        {
            float this_dist = (track_object.transform.position - ref_pos).magnitude;
            if(this_dist < dist){
                dist = this_dist;
                closest = track_object;
            }
        }

        return closest.GetComponent<AbstractTrack>();
    }

    public AbstractTrack GetTrackAt(Vector3 pos){
        GameObject track = gameObject;
        float dist2 = Mathf.Infinity;
        foreach (GameObject track_object in track_array)
        {
            float z_dist = -(track_object.transform.position.z - pos.z);
            float this_dist = (track_object.transform.position - pos).magnitude;
            if(z_dist<0){
                continue;
            }
            if(this_dist < dist2){
                dist2 = this_dist;
                track = track_object;
            }
        }
        return track.GetComponent<AbstractTrack>();
    }

    public void RequestPath(TrainController controller){

        AbstractTrack picked_track = GetTrackAt(controller.get_train_head.transform.position).GetNextTrack();
        controller.AddPath(picked_track.GetPath());
        picked_track.lock_track();
        Global.Instance.last_inspected_track = picked_track;
        
        int count = 0;
        picked_track = GetNextTrack(picked_track.gameObject);
        while(!(picked_track.gameObject.CompareTag("Junction") && picked_track.usable_junction())){
            count++;
            controller.AddPath(picked_track.GetPath());
            picked_track.lock_track();
            print(picked_track.gameObject.name);

            Global.Instance.last_inspected_track = picked_track;
            picked_track = picked_track.GetNextTrack();
            if(picked_track == null){
                Debug.LogWarning("picked track is null");
                break;
            }
            if(count>100){
                Debug.LogError("COUNT TOO HIGH. INFO: "+picked_track.gameObject.name);
                return;
            }
        }
        
    }
}
