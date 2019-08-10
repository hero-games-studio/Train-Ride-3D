using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class Global : MonoSingleton<Global>
{
    // Start is called before the first frame update
    public struct Level
    {
        public int number;
        public Segment start_station;
        public Segment end_station;

        public List<Segment> segments_in_level;

        public bool level_completed;

    }
    [HideInInspector] public Tracks tracks_object;
    [HideInInspector] public TrainController train_controller;
    [HideInInspector] public GameHandler gameManager;
    [HideInInspector] public AbstractTrack last_inspected_track;
    [HideInInspector]  public GameObject train_head;
    [HideInInspector]  public bool WaitingForTap = false;
    [HideInInspector] public float train_speed = 1f;
    [HideInInspector] public Queue<Segment> station_queue = new Queue<Segment>();
    [HideInInspector] public Level current_level{
        get{
            if(generated_levels.Count>0){
                return generated_levels.Peek();
            }
            return new Level();
        }
    }

    [HideInInspector] public Queue<Level> generated_levels;
    [HideInInspector] public Queue<AbstractTrack> junction_queue = new Queue<AbstractTrack>();
    void Start()
    {
        EventManager.StartListening("Tap",onTap);
        EventManager.StartListening("LevelFinished",Instance.LevelFinished);
        generated_levels = new Queue<Level>();
    }

    public void onTap(){
        train_controller.active = true;
    }

    public void LevelFinished(){
        Level level = generated_levels.Dequeue();
        level.level_completed = true;

        UpdateUIForLevel();
    }

    public void AddLevel(Level level){
        generated_levels.Enqueue(level);
    }

    public void UpdateUIForLevel(Level level){
        LevelBar.Instance.UpdateLeftText(level.number.ToString());
        LevelBar.Instance.UpdateRightText((level.number+1).ToString());
        LevelBar.Instance.UpdateMin(level.start_station.GetSignificantTrack().transform.position.z + 2.5f);
        LevelBar.Instance.UpdateMax(level.end_station.GetSignificantTrack().transform.position.z - 2.5f);
    }

    public void UpdateUIForLevel(){
        UpdateUIForLevel(current_level);
    }

    public void ActivateNextJunction(){
        if(junction_queue.Count >0){
            junction_queue.Dequeue().TagNextJunction();
        }
    }
}
