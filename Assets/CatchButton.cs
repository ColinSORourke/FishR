using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchButton : MonoBehaviour
{
    bool success = true;
    public PlayerData player;
    public FishingManager fishMan;

    public GameObject rewardPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fail(){
        success = false;
        this.transform.GetChild(0).GetComponent<Text>().text = "Return";
    }

    public void onClick(){
        if (success){
            Zone z = player.currZone;
            fishRarity r = z.catchFish(player.canSpecial(), 0, fishMan.currFishing);
            fishSize s = (fishSize) Random.Range(0,4);
            int p = player.getFish(r, s);
            FishObj f = z.rarityCatch(r);
            rewardPanel.GetComponent<RewardPanel>().fillOut(f, s, p, 1);
            rewardPanel.SetActive(true);
        } else {
            success = true;
            fishMan.fishingButton.interactable = true;
        }
    }
}
