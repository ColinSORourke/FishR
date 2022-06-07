using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishPanel : MonoBehaviour
{
    public fishRarity myRarity;
    public PlayerData playerData;
    public Sprite mysteryFish;
    public GameObject fullFishDisplay;
    public GameObject allFishDisplay;


    public Sprite[] metals;

    zoneData fullZone;
    FishObj myFish;
    fishData myFishData = new fishData();
    int myMetal = 0;
    string rarityText = "Common";
    bool caught = false;



    void OnEnable()
    {
        fullZone = playerData.myData.allZoneData[playerData.currZone.index];
        myFish = playerData.currZone.commonFish;

        switch (myRarity){
            case fishRarity.common:
                myFish = playerData.currZone.commonFish;
                myFishData = fullZone.commonData;
                caught = myFishData.caught;
                myMetal = 0;
                rarityText = "Common";
                break;
            case fishRarity.commonAlt:
                myFish = playerData.currZone.commonAltFish;
                myFishData = fullZone.commonAltData;
                caught = myFishData.caught;
                myMetal = 0;
                rarityText = "Common";
                break;
            case fishRarity.uncommon:
                myFish = playerData.currZone.uncommonFish;
                myFishData = fullZone.uncommonData;
                caught = myFishData.caught;
                myMetal = 1;
                rarityText = "Uncommon";
                break;
            case fishRarity.rare:
                myFish = playerData.currZone.rareFish;
                myFishData = fullZone.rareData;
                caught = myFishData.caught;
                myMetal = 2;
                rarityText = "Rare";
                break;
            case fishRarity.special:
                myFish = playerData.currZone.specialFish;
                caught = fullZone.specialCaught;
                myMetal = 2;
                rarityText = "Special";
                break;
        }

        /* var flavorText = this.transform.Find("FlavorText");
        if (caught){
            
            flavorText.GetComponent<Text>().text = myFish.description;
        } else {
            this.transform.Find("Panel/FishName").GetComponent<Text>().text = "Unknown";
            flavorText.GetComponent<Text>().text = "???\n???\n???";
        } */
        
        this.transform.Find("Panel").GetComponent<Image>().sprite = metals[myMetal];
        
        if (!caught){
            this.transform.Find("FishFrame/FishSprite").GetComponent<Image>().sprite = mysteryFish;
            this.transform.Find("Panel/FishName").GetComponent<Text>().text = "Unknown";
        } else {
            this.transform.Find("FishFrame/FishSprite").GetComponent<Image>().sprite = myFish.icon;
            this.transform.Find("Panel/FishName").GetComponent<Text>().text = myFish.fishName;
        }

        /* var caughtText = this.transform.Find("CaughtText");
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
        } */
    }

    public void fullDisplay(){
        if (caught){                
            fullFishDisplay.SetActive(true);
            allFishDisplay.SetActive(false);



            fullFishDisplay.transform.Find("Name/Text").GetComponent<Text>().text = myFish.fishName;
            fullFishDisplay.transform.Find("Name").GetComponent<Image>().sprite = metals[myMetal];
            fullFishDisplay.transform.Find("TextFrame/TextBG/Text").GetComponent<Text>().text = myFish.description;
            fullFishDisplay.transform.Find("TextFrame/TextBG").GetComponent<Image>().sprite = metals[myMetal];
            fullFishDisplay.transform.Find("Rarity/Text").GetComponent<Text>().text = rarityText;
            fullFishDisplay.transform.Find("Rarity").GetComponent<Image>().sprite = metals[myMetal];

            fullFishDisplay.transform.Find("Frame/Sprite").GetComponent<Image>().sprite = myFish.icon;

            var caughtTextLarge = fullFishDisplay.transform.Find("TextFrame/TextBG/Caught");
            if (myRarity == fishRarity.special){
                caughtTextLarge.GetComponent<Text>().text = "Caught!";
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
                caughtInfo += " | " + myFishData.numCaught + " caught";
                caughtTextLarge.GetComponent<Text>().text = caughtInfo;
            }

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