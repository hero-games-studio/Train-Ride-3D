using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.UI;
using TMPro;
using LevelInfo;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private SegmentManager segment_manager;
    
    [HideInInspector] public Queue<Level> generated_levels;
    [HideInInspector] public Level current_level{
        get{
            if(generated_levels.Count>0){
                return generated_levels.Peek();
            }
            return generated_levels.Peek();
        }
    }

    void Start()
    {
        generated_levels = new Queue<Level>();
        Input.simulateMouseWithTouches = false;
        PlayerPrefs.SetInt("coins",0);
        PlayerPrefs.SetInt("current_level",0);
        //UpdateNextJunction();

        EventManager.StartListening("CoinCollected",AddCoin);
        EventManager.StartListening("Tap",OnTap);
        EventManager.StartListening("LevelFinished",LevelFinished);

        SendNecessaryLevels();
        InitializeTrain();
        
        //ActivateQueuedLevel();
        StartCoroutine(AtStation());
    }

    private void SendNecessaryLevels(){
        List<int> needs_to_exist = new List<int>();
        needs_to_exist.Add(PlayerPrefs.GetInt("current_level"));
        needs_to_exist.Add(PlayerPrefs.GetInt("current_level") + 1);
        segment_manager.UpdateMap(needs_to_exist);
    }

    public void LevelFinished(){
        Level level = generated_levels.Dequeue();
        level.level_state = LevelState.Finished;
        PlayerPrefs.SetInt("current_level",level.number + 1);
        SendNecessaryLevels();

        StartCoroutine(AtStation());
        UpdateUIForLevel();
    }

    public void AddLevel(Level level){
        generated_levels.Enqueue(level);
    }


    public void AddCoin(){
        PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") + 1);
        //text.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void OnTap(){
        if(Global.Instance.WaitingForTap){
            EventManager.EmitEvent("TrainStart");
            Global.Instance.WaitingForTap = false;
        }
    }

    public void UpdateUIForLevel(){
        UpdateUIForLevel(current_level);
    }

    public void UpdateUIForLevel(Level level){
        LevelBar.Instance.UpdateLeftText(level.number.ToString());
        LevelBar.Instance.UpdateRightText((level.number+1).ToString());
        LevelBar.Instance.UpdateMin(level.start_station.GetSignificantTrack().transform.position.z + 2.5f);
        LevelBar.Instance.UpdateMax(level.end_station.GetSignificantTrack().transform.position.z - 2.5f);
    }

    private void ActivateQueuedLevel(){
        current_level.level_state = LevelState.Current;
        StartCoroutine(TrainMovement());
    }
    




    //TRAIN CONTROL

    

    [Header("References")]
    [SerializeField] private Tracks tracks_object;

    [Header("Prefabs")]
    [SerializeField] private GameObject train_head_prefab;
    [SerializeField] private GameObject carriage_prefab;

    private GameObject train_head;

    public GameObject get_train_head{
        get{
            return train_head;
        }
    }

    void InitializeTrain(){

        train_head = Instantiate(train_head_prefab);
        Global.Instance.train_head = train_head;
        AbstractTrack track = current_level.start_station.GetFirstTrack();
        for (int i = 0; i < 5; i++)
        {
            AddPath(track);
            track = track.GetNextTrack();
        }
        tracks_object.RequestPath(this);
        distance_travelled = 12;
        for (int i = 0; i < 3; i++)
        {
            GameObject new_carriage = Instantiate(carriage_prefab);
            train_carriages.AddLast(new_carriage);
        }
    }
    private PathSpline traverser_path = new PathSpline();
    private LinkedList<GameObject> train_carriages = new LinkedList<GameObject>();

    private float distance_travelled = 0;
    public void AddPath(AbstractTrack track){
        //paused = true;
        //path_queue.Add(path);
        //pos_map.Add(totalLength,path);
        
        PathSpline path = track.GetPath();
        LinkedList<KeyValuePair<float,PathNode>> list = new LinkedList<KeyValuePair<float,PathNode>>();
        if(path==null){
            return;
        }
        
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
                if(Mathf.Abs(temp.Value.x - traverser_path.back.x)>5 && Mathf.Abs(temp.Value.y - traverser_path.back.y)>5){
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
        Global.Instance.last_inspected_track = track;
    }

    public void AddSegment(Segment seg){
        List<AbstractTrack> list = seg.GetTracksInOrder();

        foreach (AbstractTrack item in list)
        {
            AddPath(item);
        }
    }
    
    
    float accel = 1f;
    [Header("Train Settings")]
    [SerializeField] float max_speed = 9f;
    [SerializeField] float seperation = 1.8f;

    IEnumerator TrainMovement() 
    {
        float current_time = Time.fixedTime;
        float delta_time = 0;

        float stop_seconds = 2;
        float stop_start_time = 0;

        float speed = 1.5f;
        bool manual_mode = false;
        float stop_at = 0;
        float diff;
        float newpos;
        while(current_level.level_state == LevelState.Current){
            delta_time = Time.fixedTime - current_time;
            current_time = Time.fixedTime;
            //CODE//

            if(!manual_mode && (current_level.end_station.GetSignificantTrack().getStartPoints()[0] - train_head.transform.position).magnitude <10){
                manual_mode = true;
                float temp = distance_travelled;
                Vector3 start = current_level.end_station.GetSignificantTrack().getStartPoints()[0];
                while((traverser_path.PositionAt(temp,train_head) - start).magnitude > 0.8f){
                    temp += 0.05f;
                }
                stop_at = temp;
                stop_start_time = Time.fixedTime;
            }
            
            if(!manual_mode){
                speed = Mathf.Clamp(speed + (accel*delta_time),0,max_speed);
                distance_travelled += speed*delta_time;

                train_head.transform.position = traverser_path.PositionAt(distance_travelled,train_head);
                train_head.transform.SetPositionAndRotation(train_head.transform.position, Quaternion.LookRotation(traverser_path.directionAt(distance_travelled,train_head),Vector3.up));

                if(traverser_path.totalLength - distance_travelled < 1.4f){
                    tracks_object.RequestPath(this);
                }
                
                int count = 0;
                foreach (GameObject carriage in train_carriages)
                {
                    count++;
                    carriage.transform.position = traverser_path.PositionAt(distance_travelled - count*seperation,carriage);
                    carriage.transform.SetPositionAndRotation(carriage.transform.position, Quaternion.LookRotation(traverser_path.directionAt(distance_travelled - count*seperation,carriage),Vector3.up));

                    
                }
            }else{
                diff = (current_time - stop_start_time)/(stop_seconds+0.0f);
                if(diff >1){
                    diff = 1;
                }
                float k = 7/(3+0.0f);
                //newpos = (stop_at - distance_travelled)*(-0.3f)*((8/3)*diff*diff*diff - 2*diff*diff - 4*diff)*0.75f;
                newpos = (stop_at - distance_travelled)*(diff*diff*diff/3 - (k+1)*diff*diff/2 + k*diff);
                if(diff >= 1){
                    distance_travelled += newpos;
                    LevelFinished();
                    break;
                }
                
                train_head.transform.position = traverser_path.PositionAt(distance_travelled + newpos,train_head);
                train_head.transform.SetPositionAndRotation(train_head.transform.position, Quaternion.LookRotation(traverser_path.directionAt(distance_travelled + newpos,train_head),Vector3.up));
                int count = 0;
                foreach (GameObject carriage in train_carriages)
                {
                    count++;
                    carriage.transform.position = traverser_path.PositionAt(distance_travelled + newpos - count*seperation,carriage);
                    carriage.transform.SetPositionAndRotation(carriage.transform.position, Quaternion.LookRotation(traverser_path.directionAt(distance_travelled + newpos - count*seperation,carriage),Vector3.up));

                    
                }
            }

            LevelBar.Instance.SetValue(train_head.transform.position.z);
            
            yield return null;
        }
        
        yield return null;
    }

    IEnumerator AtStation() 
    {
        Segment current_station = current_level.start_station;

        train_head.transform.position = traverser_path.PositionAt(distance_travelled,train_head);
        train_head.transform.SetPositionAndRotation(train_head.transform.position, Quaternion.LookRotation(traverser_path.directionAt(distance_travelled,train_head),Vector3.up));
        int count = 0;
        foreach (GameObject carriage in train_carriages)
        {
            count++;
            carriage.transform.position = traverser_path.PositionAt(distance_travelled- count*seperation,carriage);
            carriage.transform.SetPositionAndRotation(carriage.transform.position, Quaternion.LookRotation(traverser_path.directionAt(distance_travelled  - count*seperation,carriage),Vector3.up));        
        }
        while(true){
            if(SwipeInput.Instance.Tap){
                //Start Movement
                ActivateQueuedLevel();
                break;
            }
            yield return null;
        }

        yield return null;
    }
}
