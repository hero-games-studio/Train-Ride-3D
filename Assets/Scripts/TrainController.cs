using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using PathCreation;

public class TrainController : MonoBehaviour
{

    private GameObject train_head;
    //------//
    public int carriage_count = 2;
    public float train_speed = 5;
    public GameObject train_head_prefab;
    public GameObject carriage_prefab;

    public Tracks tracks_object;

    private List<Vector3> point_cache = new List<Vector3>();
    
    private VertexPath whole_path;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("JUNCTION_TAPPED",JunctionTapped);
        //path_list.AddLast((tracks_object.GetNextTrack() as AbstractTrack).GetPath());
        
        Vector3[] temp = PathToArray((tracks_object.GetNextTrack() as AbstractTrack).GetPath());
        foreach (Vector3 item in temp)
        {
            print(item);
            point_cache.Add(item);
        }


        BezierPath bezierPath = new BezierPath(point_cache.ToArray(),false,PathSpace.xyz);

        whole_path = new VertexPath(bezierPath,transform);

        train_head = Instantiate(train_head_prefab);
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
    public EndOfPathInstruction end;

    void Update() {
        distance_travelled += train_speed * Time.deltaTime;
        train_head.transform.position = whole_path.GetPointAtDistance(distance_travelled, end) + new Vector3(0,5,0);
        train_head.transform.rotation = whole_path.GetRotationAtDistance(distance_travelled, end);

    }
}
