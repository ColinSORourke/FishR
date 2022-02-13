using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public int cash = 100;
    public Text cashText;
    public Zone currZone;
    public zoneData[] allZoneData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeZone(Zone newZone){
        currZone = newZone;
    }

    public void getMoney(int money){
        cash += money;
        cashText.GetComponent<Text>().text = "Cash: " + cash;
    }
}

public class zoneData{
    public bool unlocked;
    public bool unlockedHint;
    public fishData commonData;
    public fishData uncommonData;
    public fishData rareData;
    public fishData specialData;
}

public class fishData{
    public bool caught;
    public int numCaught;
    public fishSize bestSize;
}

public enum fishSize{
    small = 0,
    regular = 1,
    large = 2, 
    extraLarge = 3
}
