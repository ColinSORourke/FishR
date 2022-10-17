using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseScript : MonoBehaviour
{
    public PlayerData theData;
    public ResetBoy resetter;
    public PoleManager poleMan;
    public Button confirmButton;
    public Text promptText;
    public ShopPanel POTDShop;
    public TutorialSequencer tutorial;
    public FishingManager fishMan;

    public GameObject srcButton;

    purchaseType currType;
    int purchaseCost;
    int zoneIndex;

    public void setSrc(GameObject src){
        srcButton = src;
    }

    public void zonePrompt(int i){
        currType = purchaseType.zone;
        zoneIndex = i;
        Zone theZ = theData.allZones[i];
        purchaseCost = theZ.unlockCost;
        canAfford();
        promptText.text = "Purchase the " + theZ.zoneName + " zone for " + purchaseCost + " BaitCoin?";
    }

    public void zonePurchase(){
        theData.buyZone(zoneIndex);
        srcButton.GetComponent<ZoneButton>().updateColor();
    }

    public void hintPrompt(){
        currType = purchaseType.hint;
        zoneIndex = theData.currZone.index;
        Zone theZ = theData.currZone;
        purchaseCost = theZ.hintCost;
        promptText.text = "Purchase " + theZ.zoneName + "'s hint for " + purchaseCost + " BaitCoin?";
        canAfford();
    }

    public void hintPurchase(){
        theData.buyHint(zoneIndex);
        srcButton.GetComponent<HintPanel>().unlockHint();
    }

    public void polePrompt(int budget){
        currType = purchaseType.pole;
        zoneIndex = budget;
        switch(budget){
            case 0:
                purchaseCost = 10;
                break;
            case 1: 
                purchaseCost = 40;
                break;
        }
        canAfford();
        if (poleMan.fullInv()){
            confirmButton.interactable = false;
        }
        promptText.text = "Purchase a new fishing pole for " + purchaseCost + " BaitCoin?";
    }

    public void poleOTDPrompt(){
        currType = purchaseType.poleOTD;
        purchaseCost = POTDShop.getCost();
        promptText.text = "Purchase Daily Pole for " + purchaseCost + " BaitCoin?";
        canAfford();
    }

    public void poleOTDConfirm(){
        POTDShop.purchasePOTD();
        theData.getMoney(-purchaseCost);
    }

    public void polePurchase(){
        tutorial.checkStep(3);
        theData.buyPole(purchaseCost, zoneIndex);
    }

    public void eraseAllPrompt(){
        currType = purchaseType.eraseAll;
        confirmButton.interactable = true;
        promptText.text = "Are you sure you want to erase all data?";
    }

    public void eraseAllConfirm(){
        resetter.RestartGame();
    }

    public void deletePolePrompt(){
        currType = purchaseType.deletePole;
        var p = poleMan.getPole();
        promptText.text = "Are you sure you want to delete " + p.name+ "?";
    }

    public void deletePoleConfirm(){
        poleMan.deleteCurrentPole();
    }

    public void cancelFishPrompt(){
        currType = purchaseType.cancelFish;
        promptText.text = "Cancel fishing in this zone?";
    }

    public void cancelFishConfirm(){
        fishMan.stopFishing();
    }

    public void confirmPurchase(){
        switch(currType){
            case purchaseType.zone:
                zonePurchase();
                break;
            case purchaseType.hint:
                hintPurchase();
                break;
            case purchaseType.pole:
                polePurchase();
                break;
            case purchaseType.eraseAll:
                eraseAllConfirm();
                break;
            case purchaseType.deletePole:
                deletePoleConfirm();
                break;
            case purchaseType.poleOTD:
                poleOTDConfirm();
                break;
            case purchaseType.cancelFish:
                cancelFishConfirm();
                break;
        }
    }

    public void canAfford(){
        if (theData.myData.cash < purchaseCost){
            confirmButton.interactable = false;
        } else {
            confirmButton.interactable = true;
        }
    }
}

public enum purchaseType{
    zone = 1,
    hint = 2,
    pole = 3,
    eraseAll = 4,
    deletePole = 5,
    poleOTD = 6,
    cancelFish = 7
}