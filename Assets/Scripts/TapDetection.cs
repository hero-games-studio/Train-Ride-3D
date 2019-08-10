using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class TapDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Input.simulateMouseWithTouches = false;
        gameObject.GetComponent<TouchController>().addBehaviour(TouchPhase.Began,TouchMethod);

    }

    void Update()
    {
        #if UNITY_EDITOR
        //ClickMethod();
        #endif 

        //#if UNITY_IOS
        //TouchMethod();
        //#endif
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
            if(Global.Instance.WaitingForTap){
                EventManager.EmitEvent("TrainStart");
                Global.Instance.WaitingForTap = false;
            }
            return;
        }

    }

    public static void IOSdebug(string mes){
        EventManager.SetData("debug",mes);
        EventManager.EmitEvent("debug");
    }
    private static void TouchMethod(TouchResult result)
    {
        IOSdebug(result.touch.phase.ToString());
        if (result.touch.phase == TouchPhase.Began)
        {
            IOSdebug("entered the initial if");
            Ray raycast = Camera.main.ScreenPointToRay(result.touch.position);
            RaycastHit raycastHit;
            if (Physics.Raycast(raycast, out raycastHit))
            {
                /*
                if (raycastHit.collider.CompareTag("Junction"))
                {
                    EventManager.SetData("JUNCTION_TAPPED", raycastHit.collider.gameObject);
                    EventManager.EmitEvent("JUNCTION_TAPPED");
                }elseif (raycastHit.collider.CompareTag("Interactable")){
                    IOSdebug("tapped on interactable");
                    raycastHit.collider.gameObject.GetComponent<Interactable>().OnTap();
                    return;
                }*/
            }
            if(Global.Instance.WaitingForTap){
                EventManager.EmitEvent("TrainStart");
                Global.Instance.WaitingForTap = false;
            }
            return;
                
        }
    }
}
