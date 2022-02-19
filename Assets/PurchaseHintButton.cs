using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseHintButton : MonoBehaviour
{
    public PlayerData theData;
    public Text purchaseText;

    public void OnEnable(){
        Debug.Log("This running?");
        Zone theZone = theData.currZone;
        if (theData.myData.cash < theZone.hintCost){
            this.GetComponent<Button>().interactable = false;
        } else {
             this.GetComponent<Button>().interactable = true;
        }

        purchaseText.text = "Would you like to purchase " + theZone.zoneName + "'s hint for " + theZone.hintCost + " BaitBux?";
    }
}
