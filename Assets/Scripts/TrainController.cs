using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class TrainController : MonoBehaviour
{
    private bool _CRASHED = false;
    public int framerate_target = 60;
    private GameObject train_head;

    public GameObject get_train_head{
        get{
            return train_head;
        }
    }
    private CameraController camera_controller;
    private LinkedList<GameObject> train_carriages = new LinkedList<GameObject>();
    //------//
    public int carriage_count = 2;
    public float max_train_speed = 5;
    public GameObject train_head_prefab;
    public GameObject carriage_prefab;

    private PathSpline traverser_path = new PathSpline();

    public Tracks tracks_object;


    // Start is called before the first frame update

    void InitializeTrain(){
        CalculateTotalY();

        train_head = Instantiate(train_head_prefab);
        Global.Instance.train_head = train_head;
        distance_travelled = 5;
        for (int i = 0; i < carriage_count; i++)
        {
            GameObject new_carriage = Instantiate(carriage_prefab);
            train_carriages.AddLast(new_carriage);
        }
    }
    void Start()
    {
        //Application.targetFrameRate = framerate_target;
        EventManager.StartListening("Honk",Honk);
        EventManager.StartListening("JUNCTION_TAPPED",JunctionTapped);
        camera_controller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        //find start path
        //track_list.Enqueue(tracks_object.starting_track.GetComponent<AbstractTrack>());
        AddPath(tracks_object.starting_track.GetComponent<AbstractTrack>().GetPath());
        InitializeTrain();
    }


    public void JunctionTapped(){
        GameObject junction_object = EventManager.GetGameObject("JUNCTION_TAPPED");
        if(junction_object == null){
            TapDetection.IOSdebug("JunctionTapped-- junction_object is null");
            return;
        }
        AbstractTrack junction = junction_object.GetComponent<AbstractTrack>();
        if(junction == null){
            TapDetection.IOSdebug("JunctionTapped-- junction is null");
            return;
        }
        junction.toggle_direction();
    }

    private float distance_travelled = 0;
    private Queue<AbstractTrack> track_list = new Queue<AbstractTrack>();
    private Dictionary<GameObject,float> carriage_dist_travelled_map = new Dictionary<GameObject,float>();


    public void AddPath(PathSpline path){
        //paused = true;
        //path_queue.Add(path);
        //pos_map.Add(totalLength,path);
        LinkedList<KeyValuePair<float,PathNode>> list = new LinkedList<KeyValuePair<float,PathNode>>();
        
        foreach (KeyValuePair<float,PathNode> temp in path.node_map)
        {
            list.AddLast(temp);
        }



        foreach (KeyValuePair<float,PathNode> temp in list)
        {   
            if(traverser_path.back != null){
                if(Mathf.Abs(temp.Value.x - traverser_path.back.x)<0.05 && Mathf.Abs(temp.Value.y - traverser_path.back.y)<0.05){
                    continue;
                }
            }
            //liner.positionCount = (liner.positionCount + 1);
            PathNode newnode = new PathNode(temp.Value.x,temp.Value.y,temp.Value.z);
            //vertices.Add(newnode.getVector() + new Vector3(0,5,0));
            //Point newpoint = Instantiate(linerendererobj, new Vector3(newnode.x, 2, newnode.y), Quaternion.identity).GetComponent<Point>();
            //newpoint.SetNum(count);
            //count++;
            //liner.SetPositions(vertices.ToArray());
            traverser_path.AddNode(newnode);
        }
        //paused = false;
    }
    public float seperation = 1.4f;
    private int count = 0;

    private Vector3 _up = new Vector3(0,1,0);
    void Update() {
        if(_CRASHED){
            return;
        }
        if(Time.realtimeSinceStartup>5){
            distance_travelled += CalculateCurrentSpeed() * Time.deltaTime * tracks_object.GetSpeedMultiplier(train_head);
        }

        //distance_travelled += CalculateCurrentSpeed() * Time.deltaTime;
        /*
        while(track_list.Count != 0){
            AbstractTrack last_track = track_list.Dequeue();
            print("adding " + last_track.gameObject.name);
            AddPath(last_track.GetPath());
        }*/
        if(traverser_path.totalLength - distance_travelled < 2f){
            tracks_object.RequestPath(this);

            //AbstractTrack nexttrack = tracks_object.GetNextTrack(train_head.transform.position + new Vector3(0,0,-1));
            //track_list.Enqueue(nexttrack);
            //nexttrack.lock_track();
        }
        train_head.transform.position = traverser_path.PositionAt(distance_travelled,train_head);
        train_head.transform.SetPositionAndRotation(train_head.transform.position, Quaternion.LookRotation(traverser_path.directionAt(distance_travelled,train_head),_up));
        if(camera_controller != null){
            camera_controller.UpdateCamera();
        }
        count = 0;
        foreach (GameObject carriage in train_carriages)
        {
            count++;
            carriage.transform.position = traverser_path.PositionAt(Mathf.Clamp(distance_travelled - count*seperation,0,distance_travelled),carriage);
            carriage.transform.SetPositionAndRotation(carriage.transform.position, Quaternion.LookRotation(traverser_path.directionAt(Mathf.Clamp(distance_travelled - count*seperation,0,distance_travelled),carriage),_up));

            
        }

        
    }

    /*
        Generate a speed function
     */
    private float total_y;
    private float min_y;
    private float max_y;
    private void CalculateTotalY(){
        max_y = -Mathf.Infinity;
        min_y = Mathf.Infinity;
        AbstractTrack track;
        foreach (var obj in tracks_object.get_track_array)
        {
            track = obj.GetComponent<AbstractTrack>();
            if(track.get_maxy() > max_y){
                max_y = track.get_maxy();
            }
            if(track.get_miny()  < min_y){
                min_y = track.get_miny();
            }
        }
        total_y = (max_y - min_y);
    }

    private float tanh(float t){
       return (Mathf.Exp(t) - Mathf.Exp(-t))/(Mathf.Exp(t) + Mathf.Exp(-t));
    }

    private int curve = 13;
    private float min_speed = 2;
    private float CalculateCurrentSpeed(){
        float train_y = train_head.transform.position.z - min_y;
        if(total_y < -15000 || total_y >15000){
            CalculateTotalY();
        }
        if(train_y <= min_y + total_y/2){
            return min_speed + ((max_train_speed-min_speed)*tanh(train_y/curve)*tanh(train_y/curve));
        }else{
            return min_speed + ((max_train_speed-min_speed)*tanh((total_y - train_y)/curve)*tanh((total_y - train_y)/curve));
        }
    }

    public void Crash(){
        _CRASHED = true;
        EventManager.EmitEvent("Crashed");
    }

    private void Honk(){
        train_head.transform.Find("Honk").GetComponent<AudioSource>().Play();
    }
}
