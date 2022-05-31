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
    public TutorialSequencer tutorial;

    public void fail(){
        success = false;
        this.transform.GetChild(0).GetComponent<Text>().text = "Return";
    }

    public void onClick(){
        Zone z = player.currZone;
        fishingStatus fs = fishMan.currFishing.activeInZone(z);
        FishingPole fp = poleMan.getPoleByID(fs.poleID);
        if (success){
            bool broke = poleMan.weaken(z.durCost, fp);
            fishRarity r = z.catchFish(player.canSpecial(), fp, fs);
            fishSize s;
            int sizeChance = Random.Range(1,11);
            if (sizeChance == 10){
                s = fishSize.extraLarge;
            } else if (sizeChance >= 7){
                s = fishSize.large;
            } else if (sizeChance >= 4){
                s = fishSize.medium;
            } else {
                s = fishSize.small;
            }
            int p = player.getFish(r, s);
            FishObj f = z.rarityCatch(r);
            if (fp.id == 0){
                rewardPanel.GetComponent<RewardPanel>().fillOut(f, s, p, 0);
            } else if (broke){
                rewardPanel.GetComponent<RewardPanel>().fillOut(f, s, p, -1);
            } else {
                rewardPanel.GetComponent<RewardPanel>().fillOut(f, s, p, z.durCost);
            }
            
            rewardPanel.SetActive(true);
        } else {
            tutorial.checkStep(1);
            success = true;
            this.transform.GetChild(0).GetComponent<Text>().text = "Catch!";
            fishMan.fishingButton.interactable = true;
        }
    }
}
