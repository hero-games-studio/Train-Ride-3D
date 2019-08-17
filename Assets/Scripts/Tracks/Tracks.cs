using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
public class Tracks : MonoBehaviour
{
    // Start is called before the first frame update

    //video
    //
    public Segment starting_segment;

    private List<Segment> segment_array = new List<Segment>();

    public List<Segment> get_segment_array{
        get{
            return segment_array;
        }
    }

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
            segment_array.Add(transform.GetChild(i).gameObject.GetComponent<Segment>());
            track_array.AddRange(transform.GetChild(i).gameObject.GetComponent<Segment>().GetTrackArray());
        }
        Global.Instance.tracks_object = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateArrays(){
        segment_array.Clear();
        track_array.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            segment_array.Add(transform.GetChild(i).gameObject.GetComponent<Segment>());
            track_array.AddRange(transform.GetChild(i).gameObject.GetComponent<Segment>().GetTrackArray());
        }
    }

    public AbstractTrack GetNextTrack(){
        return Global.Instance.station_queue.Peek().GetFirstTrack();
    }

    private Vector3 get_closest_point(Vector3 check_point, List<Vector3> list){
        float dist = Mathf.Infinity;
        Vector3 point = new Vector3();
        foreach (Vector3 vec in list)
        {
            if((vec-check_point).magnitude<dist){
                dist = (vec-check_point).magnitude;
                point = vec;
            }
        }
        return point;
    }

    public AbstractTrack GetNextTrack(GameObject track){
        Vector3 end_position = track.GetComponent<AbstractTrack>().GetEndPoint();

        //float x_offset =  track.GetComponent<AbstractTrack>().GetOffset();
        //float z_offset = track.GetComponent<AbstractTrack>().GetYOffset();
        //Vector3 ref_pos = track.GetComponent<AbstractTrack>().GetCenter() + new Vector3(x_offset*3,0,z_offset);

        GameObject closest = track;
        float dist = Mathf.Infinity;
        foreach (GameObject track_object in track_array)
        {
            Vector3 start_point_of_track = get_closest_point(end_position,track_object.GetComponent<AbstractTrack>().getStartPoints());
            float this_dist = (start_point_of_track - end_position).magnitude;
            if(this_dist < dist){
                dist = this_dist;
                closest = track_object;
            }
        }

        return closest.GetComponent<AbstractTrack>();
    }

    /*
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
            float z_dist = -(track_object.GetComponent<AbstractTrack>().GetStartPoint().z - pos.z);
            float this_dist = (track_object.GetComponent<AbstractTrack>().GetStartPoint() - pos).magnitude;
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

    */

    public void RequestPath(GameHandler gamehandler){
        //print("controller requested path");

        AbstractTrack picked_track = Global.Instance.last_inspected_track.GetNextTrack();
        
        //print("name of last inspected track: "+Global.Instance.last_inspected_track.name);

        //print("name of the picked_track: "+ picked_track.name);

        
        /*
        if(Global.Instance.last_inspected_track != null){
            if(!picked_track.Equals(Global.Instance.last_inspected_track)){
                controller.AddPath(picked_track);
            }
        } */
        if(picked_track==null){
            Debug.LogError("RequestPath: picked_track is null");
        }

        if(picked_track.gameObject.CompareTag("Junction")){
            picked_track.lock_track();
            //                                                  CHECK THIS
            gamehandler.AddPath(picked_track);
            picked_track = picked_track.GetNextTrack();
        }

        
        
        
        //picked_track.lock_track();
        //Global.Instance.last_inspected_track = picked_track;
        
        int count = 0;
        //picked_track = GetNextTrack(picked_track.gameObject);
        while(true){
            if((picked_track.gameObject.CompareTag("Junction") && picked_track.usable_junction())){
                Global.Instance.junction_queue.Enqueue(picked_track);
                Global.Instance.ActivateNextJunction();
                //print("queued next junction: "+ picked_track.name);
                break;
            }
            count++;

            //                                                     CHECK THIS
            gamehandler.AddPath(picked_track);
            picked_track.lock_track();

            //Global.Instance.last_inspected_track = picked_track;
            //print("adding track: "+ picked_track.name + "; next track: "+picked_track.GetNextTrack());
            picked_track = picked_track.GetNextTrack();
            if(picked_track.Equals(Global.Instance.last_inspected_track)){
                //print("found end");
                break;
            }
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

    /*
    public void RequestPath(TrainController controller, AbstractTrack track){

        AbstractTrack picked_track = track;
        if(Global.Instance.last_inspected_track != null){
            if(!picked_track.Equals(Global.Instance.last_inspected_track)){
                controller.AddPath(picked_track);
            }
        }
        if(picked_track==null){
            Debug.LogError("RequestPath: picked_track is null");
        }
        
        
        picked_track.lock_track();
        Global.Instance.last_inspected_track = picked_track;
        
        int count = 0;
        picked_track = GetNextTrack(picked_track.gameObject);
        while(!(picked_track.gameObject.CompareTag("Junction") && picked_track.usable_junction())){
            count++;
            controller.AddPath(picked_track);
            picked_track.lock_track();

            Global.Instance.last_inspected_track = picked_track;
            picked_track = picked_track.GetNextTrack();
            if(picked_track == null){
                Debug.LogWarning("picked track is null");
                break;
            }
            if(Global.Instance.last_inspected_track != null){
                if(picked_track.Equals(Global.Instance.last_inspected_track)){
                    print("found end");
                    break;
                }
            }
            
            if(count>100){
                Debug.LogError("COUNT TOO HIGH. INFO: "+picked_track.gameObject.name);
                return;
            }
        }
        
    }

    */


}
