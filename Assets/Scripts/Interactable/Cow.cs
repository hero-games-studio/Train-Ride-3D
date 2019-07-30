using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : HonkAffectee
{
    
    override public void Init(){

    }

    override public void Honk(){
        Destroy(gameObject);
    }
}
