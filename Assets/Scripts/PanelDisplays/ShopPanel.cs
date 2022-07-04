using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Random=UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    public PoleOfTheDay currPOTD;
    public PoleManager poleMan;
    public Transform POTDPanel;
    int daysInFuture = 0;
    // Start is called before the first frame update
    void Start()
    {   
        if (System.IO.File.Exists(Application.persistentDataPath + "/POTD.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/POTD.json"); 
            string JSON = reader.ReadToEnd();
            Debug.Log("Reading Pole of the day JSON");
            reader.Close();
            currPOTD = JsonUtility.FromJson<PoleOfTheDay>(JSON);
            currPOTD.checkDay();
            updateDisplay();
            save();
        } else {
            currPOTD = new PoleOfTheDay();
            currPOTD.checkDay();
            updateDisplay();
            save();
        }
    }

    public void checkDaysInFuture(){
        daysInFuture += 1;
        DateTime futureDate = DateTime.Now + new TimeSpan(daysInFuture * 24, 0 , 0);
        currPOTD.checkDay(futureDate);
        updateDisplay();
    }

    public int getCost(){
        return currPOTD.cost;
    }

    public void updateDisplay(){
        int h = currPOTD.poleStats[0];
        int b = currPOTD.poleStats[1];
        int r = currPOTD.poleStats[2];
        int c = currPOTD.poleStats[3];
        int d = currPOTD.poleStats[4];
        POTDPanel.GetChild(1).GetChild(0).GetComponent<Text>().text = "" + (5 + d);
        POTDPanel.GetChild(2).GetChild(0).GetComponent<Text>().text = "" + b;
        POTDPanel.GetChild(3).GetChild(0).GetComponent<Text>().text = "" + h;
        POTDPanel.GetChild(4).GetChild(0).GetComponent<Text>().text = "" + r;
        POTDPanel.GetChild(5).GetChild(0).GetComponent<Text>().text = "" + c;
        POTDPanel.GetChild(7).GetComponent<Button>().interactable = !currPOTD.purchased;
        if (currPOTD.purchased){
            POTDPanel.GetChild(7).GetChild(0).GetComponent<Text>().text = "Purchased!";
        } else {
            POTDPanel.GetChild(8).GetChild(0).GetComponent<Text>().text = currPOTD.cost + "";
        }
    }   

    public void purchasePOTD(){
        int h = currPOTD.poleStats[0];
        int b = currPOTD.poleStats[1];
        int r = currPOTD.poleStats[2];
        int c = currPOTD.poleStats[3];
        int d = currPOTD.poleStats[4];
        poleMan.addExactPole(h, b, r, c, d);
        poleMan.setNameIcon("Pole of the Day", 1);
        currPOTD.purchased = true;
        save();
        updateDisplay();
    }

    public void save(){
        string json = JsonUtility.ToJson(currPOTD);
        Debug.Log("Saving Pole Of The Day Data");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/POTD.json", json);
    }

    public void eraseData(){
        currPOTD = new PoleOfTheDay();
        save();
    }
}

[System.Serializable]
public class PoleOfTheDay {
    public int[] poleStats = new int[] {0, 0, 0, 0, 0};
    public serialDateTime previousDay;
    public bool purchased;
    public int cost{
        get => 5 + Mathf.FloorToInt( (poleStats[0] + poleStats[1] + poleStats[2] + poleStats[3] + poleStats[4]) * 1.5f);
    }

    public PoleOfTheDay(){
        previousDay = new serialDateTime(DateTime.Now, true);
        purchased = false;
        ulong seed = Convert.ToUInt64( previousDay.toSeed() );
        int i = 0;
        while (i < 5){
            seed = pseudoRandom(seed);
            i += 1;
        }
        i = 0;
        while (i < 5){
            seed = pseudoRandom(seed);
            poleStats[i] = Convert.ToInt32 ( seed % 11 );
            i += 1;
        }
    }

    public void checkDay(){
        serialDateTime currentDay = new serialDateTime(DateTime.Now, true);
        if (!previousDay.matchDate(currentDay)){
            Debug.Log("New Day");
            previousDay = currentDay;

            purchased = false;

            ulong seed = Convert.ToUInt64( currentDay.toSeed() );
            int i = 0;
            while (i < 5){
                seed = pseudoRandom(seed);
                i += 1;
            }
            i = 0;
            while (i < 5){
                seed = pseudoRandom(seed);
                poleStats[i] = Convert.ToInt32 ( seed % 11 );
                i += 1;
            }
        }
        
    }

    public void checkDay(DateTime check){
        serialDateTime currentDay = new serialDateTime(check, true);
        if (!previousDay.matchDate(currentDay)){
            Debug.Log("New Day");
            previousDay = currentDay;

            purchased = false;

            ulong seed = Convert.ToUInt64( currentDay.toSeed() );
            int i = 0;
            while (i < 5){
                seed = pseudoRandom(seed);
                i += 1;
            }
            i = 0;
            while (i < 5){
                seed = pseudoRandom(seed);
                poleStats[i] = Convert.ToInt32 ( seed % 11 );
                i += 1;
            }
        }
    }

    // Simple implementation of Blum Blum Shub using 50515093 as our M, and 5807, 8699 as our P&Q
    // See: https://en.wikipedia.org/wiki/Blum_Blum_Shub
    public ulong pseudoRandom(ulong seed){
        return (seed * seed) % 50515093;
    }
}


