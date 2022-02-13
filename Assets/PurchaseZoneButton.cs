using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseZoneButton : MonoBehaviour
{
    public Zone myZone;
    public ZoneButton srcButton;
    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        if (playerData.myData.cash < myZone.unlockCost){
            this.GetComponent<Button>().interactable = false;
        } else {
             this.GetComponent<Button>().interactable = true;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick(){
        Debug.Log("Purchasing Zone A");
        playerData.purchaseZone(myZone);
        srcButton.updateColor();
    }
}
