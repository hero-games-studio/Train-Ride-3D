using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public abstract class AbstractTrack : MonoBehaviour
{
    // Start is called before the first frame update
    abstract public PathCreator GetPath(int dir);
}
