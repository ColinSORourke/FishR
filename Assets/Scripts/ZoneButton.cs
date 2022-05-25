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

        this.transform.GetChild(0).GetComponent<Text>().text = myZone.zoneName;
    }

    public void onClick(){
        zoneData info = playerData.myData.allZoneData[myZone.index];
        if (!info.unlocked){
            purchasePanel.SetActive(true);
            purchasePanel.GetComponent<PurchaseScript>().zonePrompt(myZone.index);
            purchasePanel.GetComponent<PurchaseScript>().setSrc(this.gameObject);
        } else {
            playerData.changeZone(myZone);
            zonePanel.SetActive(false);
        }
    }

    public void updateColor(){
        this.GetComponent<Image>().color = new Color32(255,255,255,255);
    }
}