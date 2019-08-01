using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoSingleton<Global>
{
    // Start is called before the first frame update
    [HideInInspector]
    public Tracks tracks_object;
    [HideInInspector]
    public GameManager gameManager;
    [HideInInspector]
    public AbstractTrack last_inspected_track;
    [HideInInspector]
    public GameObject train_head;
    [HideInInspector]
    public bool WaitingForTap = false;
    [HideInInspector]
    public float train_speed = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
