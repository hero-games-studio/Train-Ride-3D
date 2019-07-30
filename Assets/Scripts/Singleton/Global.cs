using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoSingleton<Global>
{
    // Start is called before the first frame update
    public Tracks tracks_object;
    public GameManager gameManager;
    public AbstractTrack last_inspected_track;
    public GameObject train_head;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
