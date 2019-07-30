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
        gameObject.GetComponent<TouchController>().addBehaviour(TouchPhase.Began,TouchMethod); 
    }

    void Update()
    {
        #if UNITY_EDITOR
        ClickMethod();
        #endif 

        //#if UNITY_IOS
        //TouchMethod();
        //#endif
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
        next_junction.TagNextJunction();
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
        
        next_junction.TagNextJunction();
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
                else*/ if (raycastHit.collider.CompareTag("Interactable")){
                    raycastHit.collider.gameObject.GetComponent<Interactable>().OnTap();
                    return;
                }
            }
            if(next_junction!=null){
                EventManager.SetData("JUNCTION_TAPPED", next_junction.gameObject);
                EventManager.EmitEvent("JUNCTION_TAPPED");
                return;
            }
        }

    }
    private static void TouchMethod(TouchResult result)
    {
        if (result.touch.phase == TouchPhase.Began)
        {
            Ray raycast = Camera.main.ScreenPointToRay(result.touch.position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                /*
                if (raycastHit.collider.CompareTag("Junction"))
                {
                    EventManager.SetData("JUNCTION_TAPPED", raycastHit.collider.gameObject);
                    EventManager.EmitEvent("JUNCTION_TAPPED");
                }else*/ if (raycastHit.collider.CompareTag("Interactable")){
                    raycastHit.collider.gameObject.GetComponent<Interactable>().OnTap();
                    return;
                }
            }
            if(next_junction!=null){
                EventManager.SetData("JUNCTION_TAPPED", next_junction);
                EventManager.EmitEvent("JUNCTION_TAPPED");
                return;
            }
                
        }
    }
}
