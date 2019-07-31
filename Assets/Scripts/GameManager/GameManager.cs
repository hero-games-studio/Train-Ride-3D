using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    void Start()
    {
        PlayerPrefs.SetInt("coins",0);


        EventManager.StartListening("CoinCollected",AddCoin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin(){
        PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") + 1);
        text.text = PlayerPrefs.GetInt("coins").ToString();
    }
}
