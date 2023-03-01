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
        this.GetComponent<Image>().sprite = myZone.banner;
        this.transform.GetChild(0).GetComponent<Image>().sprite = myZone.title;
        if (!info.unlocked){
            this.GetComponent<Image>().color = new Color32(100,100,100,255);
        } else {
            this.GetComponent<Image>().color = new Color32(255,255,255,255);
        }
    }

    public void onClick(){
        zoneData info = playerData.myData.allZoneData[myZone.index];
        if (!info.unlocked){
            purchasePanel.SetActive(true);
            purchasePanel.GetComponent<PurchaseScript>().zonePrompt(myZone.index);
            purchasePanel.GetComponent<PurchaseScript>().setSrc(this.gameObject);
        } else {
            playerData.changeZone(myZone);
            //zonePanel.SetActive(false);
            zonePanel.GetComponent<Animator>().Play("ZoneSlideExit");
        }
    }

    public void updateColor(){
        this.GetComponent<Image>().color = new Color32(255,255,255,255);
    }
}
