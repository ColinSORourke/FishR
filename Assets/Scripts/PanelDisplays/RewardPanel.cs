using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    public GameObject rewardParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fillOut(FishObj theFish, fishSize s, int payout, int durability){
        rewardParent.transform.Find("FishImg").GetComponent<Image>().sprite = theFish.icon;
        string catchText = "You caught a";
        switch (s){
            case fishSize.small:
                catchText += " small ";
                break;
            case fishSize.medium:
                catchText += " regular ";
                break;
            case fishSize.large:
                catchText += " large ";
                break;
            case fishSize.extraLarge:
                catchText += "n EXTRA LARGE ";
                break;
        }
        catchText += theFish.fishName;
        catchText += "!";
        rewardParent.transform.Find("FishText").GetComponent<Text>().text = catchText;
        rewardParent.transform.Find("PayText").GetComponent<Text>().text = "That's worth " + payout + " BaitCoin!";
        if (durability > 0){
            rewardParent.transform.Find("FishingPole").GetComponent<Text>().text = "Your fishing pole lost " + durability + " durability.";
        } else if (durability == 0){
            rewardParent.transform.Find("FishingPole").GetComponent<Text>().text = "";
        } else if (durability == -1){
            rewardParent.transform.Find("FishingPole").GetComponent<Text>().text = "Your fishing pole ran out of durability and broke!";
        }
    }
}
