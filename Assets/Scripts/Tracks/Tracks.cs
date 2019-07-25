using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject starting_track;

    private List<GameObject> track_array = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            track_array.Add(transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AbstractTrack GetNextTrack(){
        return starting_track.GetComponent<AbstractTrack>();
    }

    public AbstractTrack GetNextTrack(GameObject track){
        print(track.name);
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
}
