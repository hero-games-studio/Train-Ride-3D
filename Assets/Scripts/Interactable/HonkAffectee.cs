using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
public abstract class HonkAffectee : MonoBehaviour
{
    // Start is called before the first frame update
    protected GameObject train_head;
    private float range = 13f;
    void Start()
    {
        EventManager.StartListening("Honk",AbstractHonk);
        Init();
    }

    virtual public void Init(){

    }

    public void AbstractHonk(){
        train_head = ((GameObject) EventManager.GetData("Honk")).transform.parent.gameObject;
        if((train_head.transform.position - transform.position).magnitude<range){
            Honk();
        }
    }

    abstract public void Honk();

    // Update is called once per frame
    void Update()
    {
        
    }
}
