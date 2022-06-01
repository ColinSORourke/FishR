using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishPanel : MonoBehaviour
{
    public fishRarity myRarity;
    public PlayerData playerData;
    public Sprite mysteryFish;
    zoneData fullZone;
    FishObj myFish;
    fishData myFishData = new fishData();

    void OnEnable()
    {
        fullZone = playerData.myData.allZoneData[playerData.currZone.index];
        myFish = playerData.currZone.commonFish;


        bool caught = false;
        switch (myRarity){
            case fishRarity.common:
                myFish = playerData.currZone.commonFish;
                myFishData = fullZone.commonData;
                caught = myFishData.caught;
                break;
            case fishRarity.commonAlt:
                myFish = playerData.currZone.commonAltFish;
                myFishData = fullZone.commonAltData;
                caught = myFishData.caught;
                break;
            case fishRarity.uncommon:
                myFish = playerData.currZone.uncommonFish;
                myFishData = fullZone.uncommonData;
                caught = myFishData.caught;
                break;
            case fishRarity.rare:
                myFish = playerData.currZone.rareFish;
                myFishData = fullZone.rareData;
                caught = myFishData.caught;
                break;
            case fishRarity.special:
                myFish = playerData.currZone.specialFish;
                caught = fullZone.specialCaught;
                break;
        }

        var flavorText = this.transform.Find("FlavorText");
        if (caught){
            this.transform.Find("Panel/FishName").GetComponent<Text>().text = myFish.fishName;
            flavorText.GetComponent<Text>().text = myFish.description;
        } else {
            this.transform.Find("Panel/FishName").GetComponent<Text>().text = "Unknown";
            flavorText.GetComponent<Text>().text = "???\n???\n???";
        }
        

        
        if (!caught){
            this.transform.Find("FishFrame/FishSprite").GetComponent<Image>().sprite = mysteryFish;
        } else {
            this.transform.Find("FishFrame/FishSprite").GetComponent<Image>().sprite = myFish.icon;
        }

        var caughtText = this.transform.Find("CaughtText");
        if (caught){
            if (myRarity == fishRarity.special){
                caughtText.GetComponent<Text>().text = "Caught!";
            } else {
                string caughtInfo = "";
                switch (myFishData.bestSize){
                    case fishSize.small: 
                        caughtInfo += "Small";
                        break;
                    case fishSize.medium: 
                        caughtInfo += "Medium";
                        break;
                    case fishSize.large: 
                        caughtInfo += "Large";
                        break;
                    case fishSize.extraLarge: 
                        caughtInfo += "XL";
                        break;
                }
                caughtInfo += " | " + myFishData.numCaught;
                caughtText.GetComponent<Text>().text = caughtInfo;
            }
        } else {
            caughtText.GetComponent<Text>().text = "Not Caught Yet";
        }
    }
}

public enum fishRarity {
    common = 0,
    commonAlt = 1,
    uncommon = 2,
    rare = 3, 
    special = 4
}