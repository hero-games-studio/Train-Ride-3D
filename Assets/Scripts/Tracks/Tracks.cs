using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject starting_track;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AbstractTrack GetNextTrack(){
        return starting_track.GetComponent<AbstractTrack>();
    }

    public AbstractTrack GetNextTrack(GameObject track){
        return starting_track.GetComponent<AbstractTrack>();
    }
}
