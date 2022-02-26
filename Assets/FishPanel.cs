using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishPanel : MonoBehaviour
{
    public fishRarity myRarity;
    public PlayerData playerData;
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

        if (specialCaught || myFishData.caught){
            this.transform.Find("FishName").GetComponent<Text>().text = myFish.fishName;
        } else {
            this.transform.Find("FishName").GetComponent<Text>().text = "Unknown";
        }
        

        this.transform.Find("FishSprite").GetComponent<Image>().sprite = myFish.icon;
        if (!specialCaught && !myFishData.caught){
            this.transform.Find("FishSprite").GetComponent<Image>().color = new Color32(8,8,8,255);
        } else {
            this.transform.Find("FishSprite").GetComponent<Image>().color = new Color32(255,255,255,255);
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

        var flavorText = this.transform.Find("FlavorText");
        if (myRarity == fishRarity.special){
            if (specialCaught){
                flavorText.GetComponent<Text>().text = myFish.description + "\n" + myFish.condition;
                this.transform.Find("HintBuy").gameObject.SetActive(false);
            } else if (fullZone.unlockedHint){
                flavorText.GetComponent<Text>().text = myFish.hint;
                this.transform.Find("HintBuy").gameObject.SetActive(false);
            } else {
                flavorText.GetComponent<Text>().text = "???\n Purchase Hint for " + playerData.currZone.hintCost + " BaitBux?\n???";
                this.transform.Find("HintBuy").gameObject.SetActive(true);
            }
        } else {
            if (myFishData.caught){
                flavorText.GetComponent<Text>().text = myFish.description;
            } else {
                flavorText.GetComponent<Text>().text = "???\n???\n???";
            }
        }
    }

    public void unlockHint(){
        this.transform.Find("FlavorText").GetComponent<Text>().text = myFish.hint;
        this.transform.Find("HintBuy").gameObject.SetActive(false);
    }
}

public enum fishRarity {
    common = 0,
    uncommon = 1,
    rare = 2, 
    special = 3
}