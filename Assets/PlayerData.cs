using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public playerSaveData myData;
    public Text cashText;
    public Zone currZone;
    public Zone[] allZones = new Zone[10];
    
    // Start is called before the first frame update
    void Start()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/PlayerData.json"); 
            string JSON = reader.ReadToEnd();
            Debug.Log(JSON);
            reader.Close();
            myData = JsonUtility.FromJson<playerSaveData>(JSON);
            cashText.GetComponent<Text>().text = "Cash: " + myData.cash;
        } else { 
            myData = new playerSaveData();
            save();
        }

        displayCurrentZone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeZone(Zone newZone){
        currZone = newZone;
        displayCurrentZone();
        save();
    }

    public void changeZone(int newZoneInd){
        currZone = allZones[newZoneInd];
        displayCurrentZone();
        save();
    }

    public void getMoney(int money){
        myData.cash += money;
        cashText.GetComponent<Text>().text = "Cash: " + myData.cash;
        save();
    }

    public void purchaseZone(Zone buyZone){
        Debug.Log("Purchasing Zone B");
        myData.cash -= buyZone.unlockCost;
        cashText.GetComponent<Text>().text = "Cash: " + myData.cash;
        myData.allZoneData[buyZone.index].unlocked = true;
        save();
    }

    public void displayCurrentZone(){
        var background = GameObject.Find("ZoneBackground");
        background.GetComponent<Image>().sprite = currZone.background;
        this.GetComponent<FishingManager>().zoneTimeText(currZone);
    }

    public void save(){
        string json = JsonUtility.ToJson(myData);

        Debug.Log("Saving as JSON: " + json);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", json);
    }
}

[Serializable]
public class playerSaveData{
    public int cash;
    public zoneData[] allZoneData;

    public playerSaveData(){
        cash = 100;
        allZoneData = new zoneData[10];
        int i = 0;
        while (i < allZoneData.Length){
            allZoneData[i] = new zoneData();
            i += 1;
        }
    }
}

[Serializable]
public class zoneData{
    public bool unlocked;
    public bool unlockedHint;
    public fishData commonData;
    public fishData uncommonData;
    public fishData rareData;
    public bool specialCaught;

    public zoneData(){
        unlocked = false;
        unlockedHint = false;
        commonData = new fishData();
        uncommonData = new fishData();
        rareData = new fishData();
        specialCaught = false;
    }
}

[Serializable]
public class fishData{
    public bool caught;
    public int numCaught;
    public fishSize bestSize;

    public fishData(){
        caught = false;
        numCaught = 0;
        bestSize = fishSize.notCaught;
    }
}

public enum fishSize{
    notCaught = -1,
    small = 0,
    regular = 1,
    large = 2, 
    extraLarge = 3
}
