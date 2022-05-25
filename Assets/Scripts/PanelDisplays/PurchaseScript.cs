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
        promptText.text = "Purchase the " + theZ.zoneName + " zone for " + purchaseCost + " BaitBux?";
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
        promptText.text = "Purchase " + theZ.zoneName + "'s hint for " + purchaseCost + " BaitBux?";
        canAfford();
    }

    public void hintPurchase(){
        theData.buyHint(zoneIndex);
        srcButton.GetComponent<HintPanel>().unlockHint();
    }

    public void polePrompt(){
        currType = purchaseType.pole;
        purchaseCost = 100;
        canAfford();
        if (poleMan.fullInv()){
            confirmButton.interactable = false;
        }
        promptText.text = "Purchase a new fishing pole for " + purchaseCost + " BaitBux?";
    }

    public void polePurchase(){
        theData.buyPole(purchaseCost);
    }

    public void eraseAllPrompt(){
        currType = purchaseType.eraseAll;
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
}