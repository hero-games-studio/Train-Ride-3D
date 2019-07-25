using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTrack : MonoBehaviour
{
    // Start is called before the first frame update
    abstract public PathSpline GetPath();

    abstract public PathSpline GetPathDir(int dir);

    abstract public int GetOffset();

    abstract public void toggle_direction();
}
