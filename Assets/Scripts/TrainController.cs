using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using PathCreation;

public class TrainController : MonoBehaviour
{

    private GameObject train_head;
    private LinkedList<GameObject> train_carriages = new LinkedList<GameObject>();
    //------//
    public int carriage_count = 2;
    public float train_speed = 5;
    public GameObject train_head_prefab;
    public GameObject carriage_prefab;

    public Tracks tracks_object;

    private Dictionary<GameObject,PathCreator> module_path_map = new Dictionary<GameObject,PathCreator>();

    // Start is called before the first frame update

    void InitializeTrain(PathCreator start_path){
        train_head = Instantiate(train_head_prefab);
        module_path_map.Add(train_head,start_path);
        distance_travelled = 2;
        for (int i = 0; i < carriage_count; i++)
        {
            GameObject new_carriage = Instantiate(carriage_prefab);
            train_carriages.AddLast(new_carriage);
            module_path_map.Add(new_carriage,start_path);
            distance_map.Add(new_carriage,distance_travelled-(i+1)*seperation);
        }
    }
    void Start()
    {
        EventManager.StartListening("JUNCTION_TAPPED",JunctionTapped);

        //find start path
        PathCreator start_path = (tracks_object.GetNextTrack() as AbstractTrack).GetPath();
        path_list.AddLast(start_path);

        InitializeTrain(start_path);
    }

    private Vector3[] PathToArray(PathCreator path){
        int res = 20;
        List<Vector3> temp = new List<Vector3>();
        float total_length = path.path.length;
        float increment = total_length/(res-1);
        for (int i = 0; i < res; i++)
        {
            temp.Add(path.path.GetPointAtDistance(increment*i,end));
        }
        return temp.ToArray();
    }

    public void JunctionTapped(){
        GameObject junction_object = EventManager.GetGameObject("JUNCTION_TAPPED");
        AbstractTrack junction = junction_object.GetComponent<AbstractTrack>();
        junction.toggle_direction();
    }

    private float distance_travelled = 0;
    private LinkedList<PathCreator> path_list = new LinkedList<PathCreator>();
    private Dictionary<GameObject,float> distance_map = new Dictionary<GameObject,float>();
    public EndOfPathInstruction end;


    public float seperation = 1.4f;
    private int count = 0;
    void Update() {
        distance_travelled += train_speed * Time.deltaTime;
        train_head.transform.position = module_path_map[train_head].path.GetPointAtDistance(distance_travelled, end);
        train_head.transform.rotation = module_path_map[train_head].path.GetRotationAtDistance(distance_travelled, end);
        count = 0;
        foreach (GameObject carriage in train_carriages)
        {
            distance_map[carriage] = distance_map[carriage]  + train_speed * Time.deltaTime;
            count++;
            carriage.transform.position = module_path_map[carriage].path.GetPointAtDistance(distance_map[carriage], end);
            carriage.transform.rotation = module_path_map[carriage].path.GetRotationAtDistance(distance_map[carriage], end);
            if(module_path_map[carriage].path.length - distance_map[carriage] < 0.0001f){
                module_path_map[carriage] = path_list.Find(module_path_map[carriage]).Next.Value;
                distance_map[carriage] = 0;
            }  
        }

        if(module_path_map[train_head].path.length - distance_travelled < 0.0001f){
            distance_travelled = 0;
            path_list.AddLast(tracks_object.GetNextTrack(path_list.Last.Value.gameObject.transform.parent.gameObject).GetPath());
            module_path_map[train_head] = path_list.Last.Value;
        }
    }
}
