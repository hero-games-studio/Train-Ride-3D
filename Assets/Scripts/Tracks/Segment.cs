using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevelInfo;

public class Segment : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public AbstractTrack significant_track;
    [SerializeField] public AbstractTrack first_track;
    public bool is_station;
    public Level currently_in_level;

    public bool gold_rich = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetSegment(){
        //Reset tracks
        List<GameObject> list = GetTrackArray();

        foreach (GameObject track in list)
        {
            track.GetComponent<AbstractTrack>().ResetTrack();
        }

        //Reset sets
        Transform sets = transform.Find("sets");
        for (int i = 0; i < sets.childCount; i++)
        {
            Transform set = sets.GetChild(i);
            Transform coins = set.Find("coins");
            Transform cows = set.Find("cows");


            //activate coins
            for (int k = 0; k < coins.childCount; k++)
            {
                coins.GetChild(k).gameObject.SetActive(true);
                coins.GetChild(k).GetComponent<Coin>().Reset();
            }

            //reset cows to initial pos
            for (int k = 0; k < cows.childCount; k++)
            {
                cows.GetChild(k).GetComponent<Cow>().Reset();
            }
        }


        //LOAD A RANDOM SET
        LoadRandomSet();
    }

    private void LoadSet(int index){
        Transform sets = transform.Find("sets");
        for (int i = 0; i < sets.childCount; i++)
        {
            if(i == index){
                sets.GetChild(i).gameObject.SetActive(true);
            }else{
                sets.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    private void LoadRandomSet(){
        if(!gold_rich){
            Transform sets = transform.Find("sets");
            System.Random rand = new System.Random();
            int index = rand.Next(0,sets.childCount);
            
            LoadSet(index);
        }else{
            Transform sets = transform.Find("sets");
            for (int i = 0; i < sets.childCount; i++)
            {
                if(sets.GetChild(i).name == "set_gr"){
                    LoadSet(i);
                }
            }
            
            return;
        }
        
    }

    public Vector2 get_start_offset(){
        Transform _start = transform.Find("_start");
        Vector3 local_pos = _start.localPosition;
        return new Vector2(local_pos.x/1.7f,local_pos.z/5);
    }

    public Vector2 get_end_offset(){
        Transform _end = transform.Find("_end");
        Vector3 local_pos = _end.localPosition;
        return new Vector2(local_pos.x/1.7f,local_pos.z/5);
    }

    private Vector3 center;
    public Vector3 getCenter(){
        if(center != null){
            return center;
        }
        int child_count = transform.Find("tracks").childCount;
        Vector3 vec_sum = new Vector3();
        for (int i = 0; i < child_count; i++)
        {
            vec_sum += transform.Find("tracks").GetChild(i).transform.position;
        }
        center = vec_sum/child_count;
        return center;
    }
    
    public List<GameObject> GetTrackArray(){
        List<GameObject> track_array = new List<GameObject>();
        for (int i = 0; i < transform.Find("tracks").childCount; i++)
        {
            track_array.Add(transform.Find("tracks").GetChild(i).gameObject);
        }
        return track_array;
    }

    public AbstractTrack GetSignificantTrack(){
        return significant_track;
    }

    public AbstractTrack GetFirstTrack(){
        if(first_track != null){
            return first_track;
        }
        Vector3 startpos = transform.Find("_start").position;
        float dist = Mathf.Infinity;
        GameObject closest = gameObject;
        for (int i = 0; i < transform.Find("tracks").childCount; i++)
        {
            if((transform.Find("tracks").GetChild(i).position - startpos).magnitude<dist){
                dist = (transform.Find("tracks").GetChild(i).position - startpos).magnitude;
                closest = transform.Find("tracks").GetChild(i).gameObject;
            }
        }

        return closest.GetComponent<AbstractTrack>();
    }

    static int sortByZ(AbstractTrack track1, AbstractTrack track2){
        return track1.transform.position.z.CompareTo(track2.transform.position.z);
    }

    public List<AbstractTrack> GetTracksInOrder(){
        List<AbstractTrack> list = new List<AbstractTrack>();
        for (int i = 0; i < transform.Find("tracks").childCount; i++)
        {
            list.Add(transform.Find("tracks").GetChild(i).GetComponent<AbstractTrack>());
        }

        list.Sort(sortByZ);
        return list;
    }
}
