using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.UI;

public class DebugMono : MonoBehaviour
{
    // Start is called before the first frame update
    public bool debug_enabled = true;
    void Start()
    {
        EventManager.StartListening("debug",Test);
    }

    LinkedList<string> messages = new LinkedList<string>();
    private int message_limit = 4;
    public void Test(){
        if(!debug_enabled){
            GetComponent<Text>().text = "";
            return;
        }
        //Add to list
        string data = EventManager.GetString("debug");
        messages.AddFirst(data);

        //print
        string toPrint = "";
        LinkedListNode<string> node = messages.First;

        int count = 0;
        do{
            toPrint = (count) + ":  " + node.Value + " \n" + toPrint;
            node = node.Next;
            count++;
        }while(count<=message_limit && node!=null);
        /*
        toPrint += "1." + node.Value + " \n";
        if(node.Next != null){
            toPrint += "2." + node.Next.Value + " \n";
            if(node.Next.Next != null){
                toPrint +="3." + node.Next.Next.Value + " \n";
            }
        }*/
        GetComponent<Text>().text = toPrint;
    }
    
}
