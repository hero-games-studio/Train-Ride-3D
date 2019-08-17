using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class Global : MonoSingleton<Global>
{
    // Start is called before the first frame update
    
    [HideInInspector] public Tracks tracks_object;
    [HideInInspector] public TrainController train_controller;
    [HideInInspector] public SegmentManager segment_manager;
    
    [HideInInspector] public GameHandler gameManager;
    [HideInInspector] public AbstractTrack last_inspected_track;
    [HideInInspector]  public GameObject train_head;
    [HideInInspector]  public bool WaitingForTap = false;
    [HideInInspector] public float train_speed = 1f;
    [HideInInspector] public Queue<Segment> station_queue = new Queue<Segment>();
    
    [HideInInspector] public Queue<AbstractTrack> junction_queue = new Queue<AbstractTrack>();
    void Start()
    {
        EventManager.StartListening("Tap",onTap);
        
    }

    public void onTap(){
        //train_controller.active = true;
    }

    

    public void ActivateNextJunction(){
        if(junction_queue.Count >0){
            junction_queue.Dequeue().TagNextJunction();
        }
    }
}
