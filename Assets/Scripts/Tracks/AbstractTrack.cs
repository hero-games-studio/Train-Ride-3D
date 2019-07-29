using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTrack : MonoBehaviour
{
    // Start is called before the first frame update
    protected AbstractTrack next_track;
    abstract public PathSpline GetPath();

    abstract public PathSpline GetPathDir(int dir);

    abstract public int GetOffset();

    abstract public void toggle_direction();

    abstract public void lock_track();

    abstract public float get_miny();
    abstract public float get_maxy();

    virtual public bool usable_junction(){
        return false;
    }

    virtual public float GetYOffset(){
        return 6f;
    }

    virtual public void CalculateNextTrack(){
        next_track = Global.Instance.tracks_object.GetNextTrack(this.gameObject);
    }

    virtual public AbstractTrack GetNextTrack(){
        return next_track;
    }

    virtual public Vector3 GetCenter(){
        return this.gameObject.transform.position + new Vector3(0,0,2.5f);
    }
}
