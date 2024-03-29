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
    public Image cashIcon;
    public Sprite[] cashIcons;
    public Zone currZone;
    public Zone[] allZones = new Zone[10];
    public TutorialSequencer tutorialMan;
    public FishingManager fishMan;

    
    void Awake(){
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerData.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/PlayerData.json"); 
            string JSON = reader.ReadToEnd();
            Debug.Log("Reading player save JSON");
            reader.Close();
            myData = JsonUtility.FromJson<playerSaveData>(JSON);
            changeZone(myData.currentZone);
        } else { 
            myData = new playerSaveData();
            save();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        updateCashText();
    }

    public void changeZone(Zone newZone){
        currZone = newZone;
        myData.currentZone = newZone.index;
        displayCurrentZone();
        save();
    }

    public void changeZone(int newZoneInd){
        currZone = allZones[newZoneInd];
        myData.currentZone = newZoneInd;
        displayCurrentZone();
        save();
    }

    public void getMoney(int money){
        myData.cash += money;
        myData.totalCashEarned += money;
        updateCashText();
        save();
    }

    public void buyZone(int zoneInd){
        Zone buyZone = allZones[zoneInd];
        myData.cash -= buyZone.unlockCost;
        updateCashText();
        myData.allZoneData[buyZone.index].unlocked = true;
        save();
    }

    public void buyZone(Zone buyZone){
        myData.cash -= buyZone.unlockCost;
        updateCashText();
        myData.allZoneData[buyZone.index].unlocked = true;
        save();
    }

    public int getFish(fishRarity theRarity, fishSize size){
        FishObj theFish = currZone.rarityCatch(theRarity);
        float payout = theFish.basePay;
        switch (size){
            case fishSize.extraLarge:
                payout *= 2.0f;
                break;
            case fishSize.large:
                payout *= 1.25f;
                break;
            case fishSize.medium:
                // Do none
                break;
            case fishSize.small:
                payout *= 0.75f;
                break;
        }
        myData.cash += Mathf.RoundToInt(payout);
        myData.totalCashEarned += Mathf.RoundToInt(payout);
        myData.totalFishCaught += 1;
        myData.recentCatchSize = size;
        updateCashText();
        if (myData.allZoneData[currZone.index].catchRarity(theRarity, size)){
            int newScore = myData.gainScore(theRarity);
            if (newScore >= 30){
                tutorialMan.checkStep(5);
                fishMan.increaseActive();
            } 
            if (newScore >= 50){
                tutorialMan.checkStep(6);
                fishMan.increaseActive();
            }
            if (newScore == 80){
                tutorialMan.checkStep(7);
            }
        }
        save();
        return Mathf.RoundToInt(payout);
    }

    public bool canSpecial(){
        return !myData.allZoneData[currZone.index].specialCaught;
    }

    public void displayCurrentZone(){
        var background = GameObject.Find("ZoneBackground");
        background.GetComponent<Image>().sprite = currZone.background;
        var fishMan = this.GetComponent<FishingManager>();
        fishMan.updateZone(currZone);
        fishMan.halfMinuteTick();
    }

    public void save(){
        string json = JsonUtility.ToJson(myData);
        Debug.Log("Saving Player Data");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", json);
    }

    public void eraseData(){
        myData = new playerSaveData();
        save();
    }

    public void buyHint(){
        myData.cash -= currZone.hintCost;
        updateCashText();
        myData.allZoneData[currZone.index].unlockedHint = true;
        save();
    }

    public void buyHint(int zoneInd){
        Zone buyZone = allZones[zoneInd];
        myData.cash -= buyZone.hintCost;
        updateCashText();
        myData.allZoneData[buyZone.index].unlockedHint = true;
        save();
    }

    public void buyPole(int cost, int budget){
        var poleMan = this.GetComponent<PoleManager>();
        myData.cash -= cost;
        poleMan.addLevelPole(budget);
        updateCashText();
        save();
    }

    public void updateCashText(){
        cashText.GetComponent<Text>().text = "x" + myData.cash;
        if (myData.cash < 100){
            cashIcon.sprite = cashIcons[0];
        } else if (myData.cash < 1000){
            cashIcon.sprite = cashIcons[1];
        } else {
            cashIcon.sprite = cashIcons[2];
        }
    }
}

[Serializable]
public class playerSaveData{
    public int cash;
    public int collectionScore;
    public int maxFishing;
    public zoneData[] allZoneData;

    public fishSize recentCatchSize;

    public int currentZone;

    public int totalCashEarned;
    public int totalFishCaught; 

    public playerSaveData(){
        cash = 9;
        totalCashEarned = 9;
        totalFishCaught = 0;
        collectionScore = 0;
        currentZone = 0;
        maxFishing = 1;
        allZoneData = new zoneData[10];
        int i = 0;
        while (i < allZoneData.Length){
            allZoneData[i] = new zoneData();
            i += 1;
        }
        allZoneData[0].unlocked = true;
    }

    public int gainScore(fishRarity r){
        switch(r){
            case fishRarity.common:
                collectionScore += 1;
                break;
            case fishRarity.commonAlt:
                collectionScore += 1;
                break;
            case fishRarity.uncommon:
                collectionScore += 2;
                break;
            case fishRarity.rare:
                collectionScore += 3;
                break;
            case fishRarity.special:
                collectionScore += 3;
                break;
        }

        if (collectionScore >= 30 && maxFishing == 1){
            maxFishing = 2;
            return collectionScore;
        }

        if (collectionScore >= 50 && maxFishing == 2){
            maxFishing = 3;
            return collectionScore;
        }

        if (collectionScore == 80){
            return collectionScore;
        }

        return 0;
    }
}

[Serializable]
public class zoneData{
    public bool unlocked;
    public bool unlockedHint;
    public int zoneScore;
    public fishData commonData;
    public fishData commonAltData;
    public fishData uncommonData;
    public fishData rareData;
    public bool specialCaught;

    public zoneData(){
        unlocked = false;
        unlockedHint = false;
        commonData = new fishData();
        commonAltData = new fishData();
        uncommonData = new fishData();
        rareData = new fishData();
        specialCaught = false;
        zoneScore = 0;
    }

    public bool catchRarity(fishRarity r, fishSize s){
        bool firstCatch = false;
        switch(r){
            case fishRarity.common:
                if (!commonData.caught){
                    commonData.caught = true;
                    firstCatch = true;
                    zoneScore += 1;
                }
                commonData.numCaught += 1;
                if (s > commonData.bestSize){
                    commonData.bestSize = s;
                }
                break;
            case fishRarity.commonAlt:
                if (!commonAltData.caught){
                    commonAltData.caught = true;
                    firstCatch = true;
                    zoneScore += 1;
                }
                commonAltData.numCaught += 1;
                if (s > commonAltData.bestSize){
                    commonAltData.bestSize = s;
                }
                break;
            case fishRarity.uncommon:
                if (!uncommonData.caught){
                    uncommonData.caught = true;
                    firstCatch = true;
                    zoneScore += 2;
                }
                uncommonData.numCaught += 1;
                if (s > uncommonData.bestSize){
                    uncommonData.bestSize = s;
                }
                break;
            case fishRarity.rare:
                if (!rareData.caught){
                    rareData.caught = true;
                    firstCatch = true;
                    zoneScore += 3;
                }
                rareData.numCaught += 1;
                if (s > rareData.bestSize){
                    rareData.bestSize = s;
                }
                break;
            case fishRarity.special:
                specialCaught = true;
                firstCatch = true;
                zoneScore += 4;
                break;
        }
        return firstCatch;
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
    medium = 1,
    large = 2, 
    extraLarge = 3
}
