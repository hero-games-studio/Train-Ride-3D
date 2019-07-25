using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpline
{
    public PathNode front;
    public PathNode back;

    public float totalLength;
    public Dictionary<float,PathNode> node_map;
    void Start(){
        node_map = new Dictionary<float,PathNode>();
    }

    public void calculateLength(){
        // REMAKE NODE_MAP
        node_map = new Dictionary<float,PathNode>();
        PathNode prev = null;
        PathNode next = front;
        float tempL = 0;
        while(next != null) {
            if(prev == null){
                node_map.Add(tempL,next);
                prev = next;
                next = next.next;
                continue;
            }
            float delta = Mathf.Sqrt((prev.x - next.x)*(prev.x - next.x) + (prev.y - next.y)*(prev.y - next.y));
            if(delta<Mathf.Epsilon){
                delta = 0;
            }
            tempL += delta;
            if(!node_map.ContainsKey(tempL)){
                node_map.Add(tempL,next);
            }
            prev = next;
            next = next.next;
        }
        totalLength = tempL;
        
    }
    public void AddNode(PathNode node){
        if(front == null){
            front = node;
        }else{
            if(back == null){
                front.next = node;
                back = node;
                back.prev = front;
            }else{
                back.next = node;
                node.prev = back;
                back = node;
            }
        }
        calculateLength();
    }

    private PathNode getNodeAt(float pos){
        PathNode prev = node_map[0];
        foreach (KeyValuePair<float,PathNode> temp in node_map)
        {
            if(pos<=temp.Key){
                break;
            }

            prev = temp.Value;
        }
        return prev;
    }

    public Vector3 PositionAt(float pos, GameObject traverser){
        if(node_map == null || node_map.Count == 0){
            return traverser.transform.position;
        }
        PathNode back = getNodeAt(pos);
        PathNode front = back.next;

        float back_pos = 0;
        float front_pos = 0;
        if(front==null){
            //Debug.Log("front is null");
            return back.getVector();
        }
        if(back==null){
            //Debug.Log("back is null");
            return front.getVector();
        }
        foreach (KeyValuePair<float,PathNode> temp in node_map)
        {
            if(back == temp.Value){
                back_pos = temp.Key;
            }
            if(front == temp.Value){
                front_pos = temp.Key;
            }
        }
        float ratio = (pos-back_pos)/(front_pos - back_pos);
            
        return ((back.getVector()*(1-ratio)) + front.getVector()*(ratio) );
    }

    public Vector3 directionAt(float pos, GameObject traverser){
        if(node_map == null || node_map.Count == 0){
            return traverser.transform.position;
        }
        Vector3 start = PositionAt(pos-0.5f,traverser);
        Vector3 end = PositionAt(pos+0.5f,traverser);
        return (end-start).normalized;
    }
}
