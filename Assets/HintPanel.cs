using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintPanel : MonoBehaviour
{
    public PlayerData playerData;
    zoneData fullZone;
    bool specialCaught = false;

    // Start is called before the first frame update
    void OnEnable(){
        fullZone = playerData.myData.allZoneData[playerData.currZone.index];
        specialCaught = fullZone.specialCaught;

        if (fullZone.specialCaught){
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerData.currZone.fullHint;
        }
        else if (fullZone.unlockedHint){
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerData.currZone.hint;
        }
    }

    public void unlockHint(){
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(true);
        this.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerData.currZone.hint; 
    }
}
