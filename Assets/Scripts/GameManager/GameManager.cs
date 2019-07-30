using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("coins")){
            PlayerPrefs.SetInt("coins",0);
        }

        EventManager.StartListening("CoinCollected",AddCoin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin(){
        PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") + 1);
    }
}
