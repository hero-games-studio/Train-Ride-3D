using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//Single finger basic touch controller
public class TouchController : MonoBehaviour
{
    //Dictionary for different touch phases 
    public Dictionary<TouchPhase, Behaviour> behaviourDictionary = new Dictionary<TouchPhase, Behaviour>();
    public bool isProcessClickOverUI = false;
    private TouchResult result = new TouchResult();


    //Add new behaviour type for checking in update
    public void addBehaviour(TouchPhase phase, Behaviour.TapDelegate newDelegate)
    {
        if (behaviourDictionary.ContainsKey(phase))
        {
            behaviourDictionary[phase].addDelegateIfNotExist(newDelegate);
        }
        else
        {
            behaviourDictionary[phase] = new Behaviour();
            behaviourDictionary[phase].addDelegateIfNotExist(newDelegate);
        }
    }


    //Check touches
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            //If pointer is not over ui element check but touch phase ended still works be carefull
            if (!isProcessClickOverUI && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (behaviourDictionary.ContainsKey(touch.phase))
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        result.firstPressPosition = touch.position;
                    }
                    result.touch = touch;
                    behaviourDictionary[touch.phase].processDelegates(result);
                }
                else
                {
                    addBehaviour(touch.phase, null);
                }
            }
        }
    }

}
//Container for delegates and selected phase
public class Behaviour
{
    public delegate void TapDelegate(TouchResult result);
    private List<TapDelegate> delegates = new List<TapDelegate>();

    public bool addDelegateIfNotExist(TapDelegate newDelegate)
    {
        if (delegates.Contains(newDelegate))
        {
            return false;
        }
        else
        {
            delegates.Add(newDelegate);
        }
        return true;
    }

    public void processDelegates(TouchResult touch)
    {
        foreach (TapDelegate tempDelegate in delegates)
        {
            if (tempDelegate != null)
            {
                tempDelegate(touch);
            }
        }
    }
}

//For holding firstpressRotation
public class TouchResult
{
    public Touch touch;
    public Vector3 firstPressPosition;
}

