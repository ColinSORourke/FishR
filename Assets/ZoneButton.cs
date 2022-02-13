using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoneButton : MonoBehaviour
{
    public Zone myZone;
    public PlayerData playerData;
    public GameObject purchasePanel;
    public GameObject zonePanel;


    // Start is called before the first frame update
    void Start()
    {
        zoneData info = playerData.myData.allZoneData[myZone.index];
        if (!info.unlocked){
            this.GetComponent<Image>().color = new Color32(100,100,100,200);
        } else {
            this.GetComponent<Image>().color = new Color32(255,255,255,255);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick(){
        zoneData info = playerData.myData.allZoneData[myZone.index];
        if (!info.unlocked){
            purchasePanel.SetActive(true);
            var purchaseText = purchasePanel.transform.GetChild(3);
            var purchaseButton = purchasePanel.transform.GetChild(1);
            purchaseButton.GetComponent<PurchaseZoneButton>().myZone = myZone;
            purchaseButton.GetComponent<PurchaseZoneButton>().srcButton = this;
            purchaseText.GetComponent<Text>().text = "Do you want to purchase the " + myZone.zoneName +" zone for " + myZone.unlockCost + " BaitBux?";
        } else {
            playerData.changeZone(myZone);
            zonePanel.SetActive(false);
        }
    }

    public void updateColor(){
        this.GetComponent<Image>().color = new Color32(255,255,255,255);
    }
}
