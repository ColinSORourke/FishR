using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using System.IO;
using System;

public class PoleManager : MonoBehaviour
{
    public playerPoleSave myPoles;  
    public GameObject poleDisplay;
    public GameObject PDLObj;
    private PoleDisplayLarge PDL;
    public FishingManager catchMan; 
    // Start is called before the first frame update
    void Awake(){
        PDL = PDLObj.GetComponent<PoleDisplayLarge>();
    }
    
    void Start()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/PlayerPoles.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/PlayerPoles.json"); 
            string JSON = reader.ReadToEnd();
            Debug.Log("Reading player save JSON");
            reader.Close();
            myPoles = JsonUtility.FromJson<playerPoleSave>(JSON);
        } else { 
            myPoles = new playerPoleSave();
            save();
        }

        if (myPoles.myPolesLen > 0){
            poleDisplay.transform.Find("Buttons").gameObject.SetActive(true);
            PDL.buttonsActive(true);
        }
        this.updatePoleDisplay();
    }

    public void save(){
        string json = JsonUtility.ToJson(myPoles);
        Debug.Log("Saving Player Poles");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerPoles.json", json);
    }

    public FishingPole getPole(){
        return myPoles.getPole();
    }

    public void left(){
        FishingPole result = myPoles.left();
        save();
        this.updatePoleDisplay();
    }

    public void right(){
        FishingPole result = myPoles.right();
        save();
        this.updatePoleDisplay();
    }

    public void weaken(int dur, FishingPole p){
        int pIndex = myPoles.find(p);
        myPoles.weaken(dur, pIndex);
        save();
        if (myPoles.myPolesLen == 0){
            poleDisplay.transform.Find("Buttons").gameObject.SetActive(false);
            PDL.buttonsActive(false);
        }
        this.updatePoleDisplay();
    }

    public void addRandomPole(){
        myPoles.addRandomPole();
        save();
        poleDisplay.transform.Find("Buttons").gameObject.SetActive(true);
        PDL.buttonsActive(true);
    }

    public void deleteRandomPole(){
        myPoles.removeRandomPole();
        save();
        if (myPoles.myPolesLen == 0){
            poleDisplay.transform.Find("Buttons").gameObject.SetActive(false);
            PDL.buttonsActive(false);
        }
        this.updatePoleDisplay();
    }

    public void deleteCurrentPole(){
        myPoles.removeCurrentPole();
        save();
        if (myPoles.myPolesLen == 0){
            poleDisplay.transform.Find("Buttons").gameObject.SetActive(false);
            PDL.buttonsActive(false);
        }
        this.updatePoleDisplay();
    }

    public void updatePoleDisplay(){
        FishingPole p = this.getPole();
        Debug.Log(PDL);
        PDL.displayPole(p);
        poleDisplay.transform.Find("LuckText").GetComponent<Text>().text = p.charm + "";
        poleDisplay.transform.Find("ReelText").GetComponent<Text>().text = p.reel + "";
    }
}

[Serializable]
public class playerPoleSave {
    public List<FishingPole> myPoles = new List<FishingPole>();
    public int myPolesLen;
    public FishingPole basePole = new FishingPole(0);
    public int currSelectPole = -1;
    public int polesBought = 0;

    public FishingPole getPole(){
        if (currSelectPole != -1){
            return myPoles[currSelectPole];
        } else {
            return basePole;
        }
    }

    public FishingPole left(){
        currSelectPole = currSelectPole == -1 ? myPolesLen -1 : currSelectPole -1;
        return getPole();
    }

    public FishingPole right(){
        currSelectPole = currSelectPole == myPolesLen - 1 ? -1 : currSelectPole + 1;
        return getPole();
    }

    public void weaken(int durLoss, int pole = -8){
        if (pole == -8){
            pole = currSelectPole;
        }
        if (pole != -1){
            if (myPoles[pole].weaken(durLoss)){
                this.remove(pole);
            }
        }
    }

    public void remove(int toRemove){
        myPoles.RemoveAt(toRemove);
        myPolesLen -= 1;
        currSelectPole = -1;
    }

    public int find(FishingPole toFind){
        if (toFind.id != 0){
            return myPoles.FindIndex(x => x.id == toFind.id);
        } else {
            return -1;
        }
    }

    public void addRandomPole(){
        if (myPolesLen < 5){
            polesBought += 1;
            int h = Random.Range(0, 21);
            int b = Random.Range(0, 21);
            int r = Random.Range(0, 21);
            int c = Random.Range(0, 21);
            int d = Random.Range(0, 21);

            FishingPole newPole = new FishingPole(polesBought, h, b, r, c, d);
            myPoles.Add(newPole);
            myPolesLen += 1;
        }
    }

    public void removeRandomPole(){
        if (myPolesLen > 0){
            int toRemove = Random.Range(0, myPolesLen);
            this.remove(toRemove);
        }
    }

    public void removeCurrentPole(){
        if (myPolesLen > 0 && currSelectPole != -1){
            this.remove(currSelectPole);
        }
    }
}

[Serializable]
public class FishingPole {
    public int id;
    public string name;
    public int hook;
    public int bait;
    public int reel;
    public int charm;
    public int durability;
    public int currDur;
    // public item bobber;
    // public tier rank;
    
    public FishingPole(int i){ 
        name = "Basic Pole";
        id = i;
        hook = 0;
        bait = 0;
        reel = 0;
        charm = 0;
        durability = -1;
        currDur = -1;
    }

    public FishingPole(int i, int Budget){
        // TO IMPLEMENT
    }

    public FishingPole(int i, int h, int b, int r, int c, int d){
        name = "Custom Pole " + i;
        id = i;
        hook = h;
        bait = b;
        reel = r;
        charm = c;
        durability = 10 + d;
        currDur = durability;
    }

    public void addItem(){
        // TO IMPLEMENT
    }

    public bool weaken(int durLoss){
        currDur -= durLoss;
        return (currDur <= 0);
    }
}
