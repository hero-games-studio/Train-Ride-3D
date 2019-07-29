using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class TapDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UpdateNextJunction();
    }

    void Update()
    {
        #if UNITY_EDITOR
        ClickMethod();
        #endif 

        #if UNITY_IOS
        TouchMethod();
        #endif
    }

    private static AbstractTrack next_junction;

    public static void UpdateNextJunction(){
        Tracks tracks_obj = Global.Instance.tracks_object;
        Vector3 initial_pos = Global.Instance.train_head.transform.position;

        AbstractTrack picked_track = tracks_obj.GetNextTrack(initial_pos);
        while(!picked_track.gameObject.CompareTag("Junction") || !picked_track.usable_junction()){
            picked_track = tracks_obj.GetNextTrack(picked_track.gameObject);
            if(picked_track == null){
                Debug.LogWarning("picked track is null");
                break;
            }
        }
        next_junction = picked_track;
    }
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
    }

    private static void ClickMethod()
    {   
        
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(raycast, out raycastHit);
        if(Input.GetMouseButtonDown(0)){
            if (Physics.Raycast(raycast, out raycastHit))
            {
                /*
                if (raycastHit.collider.CompareTag("Junction"))
                {
                    EventManager.SetData("JUNCTION_TAPPED", raycastHit.collider.gameObject);
                    EventManager.EmitEvent("JUNCTION_TAPPED");
                }
                else*/ if (raycastHit.collider.CompareTag("Rocks")){
                    raycastHit.collider.gameObject.GetComponent<Rocks>().OnTap();
                }else if (raycastHit.collider.CompareTag("Bush")){
                    raycastHit.collider.gameObject.GetComponent<Bushes>().OnTap();
                } 
            }
            if(next_junction!=null){
                EventManager.SetData("JUNCTION_TAPPED", next_junction.gameObject);
                EventManager.EmitEvent("JUNCTION_TAPPED");
            }
        }

    }
    private static void TouchMethod()
    {
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                /*
                if (raycastHit.collider.CompareTag("Junction"))
                {
                    EventManager.SetData("JUNCTION_TAPPED", raycastHit.collider.gameObject);
                    EventManager.EmitEvent("JUNCTION_TAPPED");
                }else*/ if (raycastHit.collider.CompareTag("Rocks")){
                    raycastHit.collider.gameObject.GetComponent<Rocks>().OnTap();
                }else if (raycastHit.collider.CompareTag("Bush")){
                    raycastHit.collider.gameObject.GetComponent<Bushes>().OnTap();
                }
            }
            if(next_junction!=null){
                EventManager.SetData("JUNCTION_TAPPED", next_junction);
                EventManager.EmitEvent("JUNCTION_TAPPED");
            }
                
        }
    }
}
