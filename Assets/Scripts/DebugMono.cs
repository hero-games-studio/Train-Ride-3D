using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.UI;

public class DebugMono : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("touch",Test);
    }

    private int count = 0;
    public void Test(){
        count++;
        GetComponent<Text>().text = count.ToString();
    }
    
}
