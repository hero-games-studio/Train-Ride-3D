using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    // Start is called before the first frame update
    private float _x;
    private float _y;

    private float _z = 0;

    public PathNode prev;
    public PathNode next;
    

    public float x{
        get{
            return _x;
        }
    }

    public float y{
        get{
            return _y;
        }
    }

    public float z{
        get{
            return _z;
        }
    }

    public bool hasNext(){
        if(next != null){
            return true;
        }
        return false;
    }
    public PathNode(float ax, float ay){
        _x = ax;
        _y = ay;
    }

    public PathNode(float ax, float ay, float az){
        _x = ax;
        _y = ay;
        _z = az;
    }

    public PathNode(PathNode prev,float ax, float ay){
        _x = ax;
        _y = ay;
        this.prev = prev;
    }

    public PathNode(float ax, float ay, PathNode next){
        _x = ax;
        _y = ay;
        this.next = next;
    }

     public PathNode(PathNode prev, float ax, float ay, PathNode next){
        _x = ax;
        _y = ay;
        this.prev = prev;
        this.next = next;
    }

    public Vector3 getVector(){
        return new Vector3(_x,_z,_y);
    }
}
