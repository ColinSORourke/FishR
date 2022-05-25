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
    bool specialCaught = false;

    void OnEnable()
    {
        fullZone = playerData.myData.allZoneData[playerData.currZone.index];
        myFish = playerData.currZone.commonFish;

        switch (myRarity){
            case fishRarity.common:
                myFish = playerData.currZone.commonFish;
                myFishData = fullZone.commonData;
                break;
            case fishRarity.uncommon:
                myFish = playerData.currZone.uncommonFish;
                myFishData = fullZone.uncommonData;
                break;
            case fishRarity.rare:
                myFish = playerData.currZone.rareFish;
                myFishData = fullZone.rareData;
                break;
            case fishRarity.special:
                myFish = playerData.currZone.specialFish;
                specialCaught = fullZone.specialCaught;
                break;
        }

        var flavorText = this.transform.Find("FlavorText");
        if (specialCaught || myFishData.caught){
            this.transform.Find("FishName").GetComponent<Text>().text = myFish.fishName;
            flavorText.GetComponent<Text>().text = myFish.description;
        } else {
            this.transform.Find("FishName").GetComponent<Text>().text = "Unknown";
            flavorText.GetComponent<Text>().text = "???\n???\n???";
        }
        

        
        if (!specialCaught && !myFishData.caught){
            this.transform.Find("FishSprite").GetComponent<Image>().sprite = mysteryFish;
        } else {
            this.transform.Find("FishSprite").GetComponent<Image>().sprite = myFish.icon;
        }

        var caughtText = this.transform.Find("CaughtText");
        if (specialCaught){
            caughtText.GetComponent<Text>().text = "Caught!";
        } else if (myFishData.caught){
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
        } else {
            caughtText.GetComponent<Text>().text = "Not Caught Yet";
        }
    }
}

public enum fishRarity {
    common = 0,
    uncommon = 1,
    rare = 2, 
    special = 3
}