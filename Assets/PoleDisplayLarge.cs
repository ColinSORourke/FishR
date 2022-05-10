using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoleDisplayLarge : MonoBehaviour
{
    public GameObject Durability;
    public Transform Bait;
    public Transform Hook;
    public Transform Charm;
    public Transform Reel;
    public Text Name;
    public GameObject Item;
    public GameObject Buttons;
    public GameObject DestroyButton;
    public GameObject inUseIcon;

    public void buttonsActive(bool a){
        Buttons.SetActive(a);
    }

    public void displayPole(FishingPole fp){
        if (fp.id != 0){
            showStats(true);
            Name.text = fp.name;

            // Pares Pole Icon HERE
            RectTransform durRT = Durability.GetComponent<RectTransform>();
            durRT.sizeDelta = new Vector2 (fp.durability * 20, durRT.sizeDelta.y);
            Durability.transform.GetChild(0).GetComponent<Image>().fillAmount = fp.currDur / ( 0.0f + fp.durability );
            Durability.transform.GetChild(1).GetComponent<Text>().text = "Durability: " + fp.currDur + "/" + fp.durability;

            Bait.GetChild(0).gameObject.GetComponent<Image>().fillAmount = fp.bait / 20.0f;
            Bait.GetChild(2).gameObject.GetComponent<Text>().text = fp.bait + "\nBait";
            Hook.GetChild(0).gameObject.GetComponent<Image>().fillAmount = fp.hook / 20.0f;
            Hook.GetChild(2).gameObject.GetComponent<Text>().text = fp.hook + "\nHook";
            Reel.GetChild(0).gameObject.GetComponent<Image>().fillAmount = fp.reel / 20.0f;
            Reel.GetChild(2).gameObject.GetComponent<Text>().text = fp.reel + "\nReel";
            Charm.GetChild(0).gameObject.GetComponent<Image>().fillAmount = fp.charm / 20.0f;
            Charm.GetChild(2).gameObject.GetComponent<Text>().text = fp.charm + "\nCharm";

            // Parse Item HERE
        } else {
            showStats(false);
            Name.text = fp.name;
        }
    }

    public void showStats(bool a){
        Durability.SetActive(a);
        Bait.gameObject.SetActive(a);
        Hook.gameObject.SetActive(a);
        Reel.gameObject.SetActive(a);
        Charm.gameObject.SetActive(a);
        Item.SetActive(a);
        DestroyButton.SetActive(a);
    }

    public void inUse(bool a){
        inUseIcon.SetActive(a);
    }
}
