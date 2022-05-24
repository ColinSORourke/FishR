using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public PlayerData playerData;

    void OnEnable(){
        Text myText = this.transform.GetChild(0).GetComponent<Text>();
        zoneData fullZone = playerData.myData.allZoneData[playerData.currZone.index];
        myText.text = "Collection Score: " + playerData.myData.collectionScore + "\nScore this Zone: " + fullZone.zoneScore + "/10";
    }
}
