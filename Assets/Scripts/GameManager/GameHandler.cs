using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.UI;
using TMPro;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI text;
    void Start()
    {
        Input.simulateMouseWithTouches = false;
        PlayerPrefs.SetInt("coins",0);
        //UpdateNextJunction();

        EventManager.StartListening("CoinCollected",AddCoin);
        EventManager.StartListening("Tap",OnTap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static AbstractTrack next_junction;

    /*
    public static void UpdateNextJunction(){
        int tracker = 0;
        Tracks tracks_obj = Global.Instance.tracks_object;
        Vector3 initial_pos = Global.Instance.train_head.transform.position;

        AbstractTrack picked_track = tracks_obj.GetNextTrack(initial_pos);
        while(!picked_track.gameObject.CompareTag("Junction") || !picked_track.usable_junction()){
            tracker++;
            picked_track = tracks_obj.GetNextTrack(picked_track.gameObject);
            if(picked_track == null){
                Debug.LogWarning("picked track is null");
                break;
            }

            if(tracker>100){
                Debug.LogError("GameHandler/UpdateNextJunction: Infinite Loop");
            }
        }
        next_junction = picked_track;
        next_junction.TagNextJunction();
    }

    */

    /*
    public static void UpdateNextJunction(AbstractTrack track){
        Tracks tracks_obj = Global.Instance.tracks_object;
        Vector3 initial_pos = Global.Instance.train_head.transform.position;

        int count = 0;
        AbstractTrack picked_track = tracks_obj.GetNextTrack(track.gameObject);
        while(!picked_track.gameObject.CompareTag("Junction") || !picked_track.usable_junction()){
            count++;
            picked_track = tracks_obj.GetNextTrack(picked_track.gameObject);
            if(picked_track == null){
                Debug.LogWarning("picked track is null");
                break;
            }
            if(count>100){
                Debug.LogError("NEXT JUNCTION INFINITE");
                return;
            }
        }
        next_junction = picked_track;
        
        next_junction.TagNextJunction();
    }

    */


    public void AddCoin(){
        PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") + 1);
        text.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void OnTap(){
        if(Global.Instance.WaitingForTap){
            EventManager.EmitEvent("TrainStart");
            Global.Instance.WaitingForTap = false;
        }
    }
}
