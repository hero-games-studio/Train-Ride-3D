using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class TapDetection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

    private static void ClickMethod()
    {   
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Physics.Raycast(raycast, out raycastHit);
        if(Input.GetMouseButtonDown(0)){
            if (Physics.Raycast(raycast, out raycastHit))
            {
                if (raycastHit.collider.CompareTag("Junction"))
                {
                    EventManager.SetData("JUNCTION_TAPPED", raycastHit.collider.gameObject);
                    EventManager.EmitEvent("JUNCTION_TAPPED");
                }
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

                if (raycastHit.collider.CompareTag("Junction"))
                {
                    EventManager.SetData("JUNCTION_TAPPED", raycastHit.collider.gameObject);
                    EventManager.EmitEvent("JUNCTION_TAPPED");
                }
            }
        }
    }
}
