using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchButton : MonoBehaviour
{
    bool success = true;
    public PlayerData player;
    public FishingManager fishMan;
    public PoleManager poleMan;

    public GameObject rewardPanel;

    public void fail(){
        success = false;
        this.transform.GetChild(0).GetComponent<Text>().text = "Return";
    }

    public void onClick(){
        Zone z = player.currZone;
        fishingStatus fs = fishMan.currFishing.activeInZone(z);
        FishingPole fp = poleMan.getPoleByID(fs.poleID);
        if (success){
            poleMan.weaken(z.durCost, fp);
            fishRarity r = z.catchFish(player.canSpecial(), fp, fs);
            fishSize s = (fishSize) Random.Range(0,4);
            int p = player.getFish(r, s);
            FishObj f = z.rarityCatch(r);
            rewardPanel.GetComponent<RewardPanel>().fillOut(f, s, p, z.durCost);
            rewardPanel.SetActive(true);
        } else {
            success = true;
            fishMan.fishingButton.interactable = true;
        }
        poleMan.stopUsing(fs.poleID);
    }
}
