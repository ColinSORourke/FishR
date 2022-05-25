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
    public int lockedPole = -2;

    public Sprite[] poleSprites = new Sprite[10];
    // Start is called before the first frame update
    void Awake(){
        PDL = PDLObj.GetComponent<PoleDisplayLarge>();
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
    }
    
    void Start()
    {
        if (myPoles.myPolesLen > 0){
            poleDisplay.transform.Find("Buttons").gameObject.SetActive(true);
            PDL.buttonsActive(true);
        } else {
            poleDisplay.transform.Find("Buttons").gameObject.SetActive(false);
            PDL.buttonsActive(false);
        }
        this.updatePoleDisplay();
    }

    public void eraseData(){
        myPoles = new playerPoleSave();
        save();
    }

    public void save(){
        string json = JsonUtility.ToJson(myPoles);
        Debug.Log("Saving Player Poles");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerPoles.json", json);
    }

    public FishingPole getPole(){
        return myPoles.getPole();
    }

    public FishingPole getPole(int ind){
        return myPoles.getPole(ind);
    }

    public FishingPole getPoleByID(int id){
        int index = myPoles.findByID(id);
        return myPoles.getPole(index);
    }

    public void left(){
        FishingPole result = myPoles.left();
        save();
        this.updatePoleDisplay();
        catchMan.updatePole(result, myPoles.inUseQ());
    }

    public void right(){
        FishingPole result = myPoles.right();
        save();
        this.updatePoleDisplay();
        catchMan.updatePole(result, myPoles.inUseQ());
    }

    public void weakenByID(int dur, int poleID){
        FishingPole myPole = this.getPoleByID(poleID);
        this.weaken(dur, myPole);
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
        this.updatePoleDisplay();
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
        PDL.displayPole(p, getSprite(p.spriteID));
        poleDisplay.GetComponent<Image>().sprite = getSprite(p.spriteID);
        PDL.inUse(this.myPoles.inUseQ());

        if (lockedPole == -2){
            poleDisplay.transform.Find("LuckColor").Find("LuckText").GetComponent<Text>().text = p.charm + "";
            poleDisplay.transform.Find("ReelColor").Find("ReelText").GetComponent<Text>().text = p.reel + "";
            poleDisplay.transform.Find("NameBG").Find("NameText").GetComponent<Text>().text = p.name;
            poleDisplay.transform.Find("InUse").gameObject.SetActive(myPoles.inUseQ());
        } else {
            poleDisplay.transform.Find("Buttons").gameObject.SetActive(false);
        }
    }

    public bool inUse(){
        return myPoles.inUseQ();
    }

    public void startUsing(){
        myPoles.startUsing();
        this.updatePoleDisplay();
        save();
    }

    public void stopUsing(int poleID){
        Debug.Log("No Longer using pole ID: " + poleID);
        int ind = myPoles.findByID(poleID);
        myPoles.stopUsing(ind);
        save();
        this.updatePoleDisplay();
    }

    public void lockPole(int poleID){
        lockedPole = myPoles.findByID(poleID);
        FishingPole p = myPoles.getPole(lockedPole);

        myPoles.currSelectPole = lockedPole;
        poleDisplay.transform.Find("LuckColor").Find("LuckText").GetComponent<Text>().text = p.charm + "";
        poleDisplay.transform.Find("ReelColor").Find("ReelText").GetComponent<Text>().text = p.reel + "";
        poleDisplay.transform.Find("NameBG").Find("NameText").GetComponent<Text>().text = p.name;
        poleDisplay.transform.Find("InUse").gameObject.SetActive(myPoles.inUseQ());
        poleDisplay.transform.Find("Buttons").gameObject.SetActive(false);
    }

    public void unlockPole(){
        lockedPole = -2;
        if (myPoles.myPolesLen > 0){
            poleDisplay.transform.Find("Buttons").gameObject.SetActive(true);
        }
        this.updatePoleDisplay();
    }

    public Sprite getSprite(int id){
        if (id < poleSprites.Length){
            return poleSprites[id];
        }
        return poleSprites[0];
    }

    public bool fullInv(){
        return myPoles.myPolesLen == 5;
    }
}

[Serializable]
public class playerPoleSave {
    public List<FishingPole> myPoles = new List<FishingPole>();
    public bool[] inUse = new bool[6];
    public int myPolesLen;
    public FishingPole basePole = new FishingPole(0);
    public int currSelectPole = -1;
    public int polesBought = 0;

    public playerPoleSave(){
        int i = 0;
        while (i < 6){
            inUse[i] = false;
            i += 1;
        }
    }

    public FishingPole getPole(){
        if (currSelectPole != -1){
            return myPoles[currSelectPole];
        } else {
            return basePole;
        }
    }

    public FishingPole getPole(int ind){
        if (ind < myPoles.Count && ind >= 0){
            return myPoles[ind];
        } else {
            return basePole;
        }
    }

    public void startUsing(){
        inUse[currSelectPole + 1] = true;
    }

    public void startUsing(int ind){
        if (ind < 6){
            inUse[ind + 1] = true;
        } 
    }

    public void stopUsing(int ind){
        Debug.Log("No Longer using pole index: " + ind);
        if (ind < 6){
            inUse[ind + 1] = false;
        } 
    }

    public bool inUseQ(){
        return inUse[currSelectPole + 1];
    }

    public bool inUseQ(int ind) {
        if (ind < 6){
            return inUse[ind + 1];
        } 
        return false;
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

    public int findByID(int i){
        return myPoles.FindIndex(x => x.id == i);
    }

    public void addRandomPole(){
        if (myPolesLen < 5){
            polesBought += 1;
        
            FishingPole newPole = new FishingPole(polesBought, 0);
            myPoles.Add(newPole);
            currSelectPole = myPolesLen;
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
    public int[] stats = new int[] {0,0,0,0,0};

    public int hook{
        get => stats[0];
        set {
            stats[0] = value;
        }
    }
    public int bait{
        get => stats[1];
        set {
            stats[1] = value;
        }
    }
    public int reel{
        get => stats[2];
        set {
            stats[2] = value;
        }
    }
    public int charm{
        get => stats[3];
        set {
            stats[3] = value;
        }
    }
    public int durability{
        get => stats[4] + 5;
        set {
            stats[4] = value - 5;
        }
    }
    public int currDur;
    public int spriteID;
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
        spriteID = 0;
    }

    public FishingPole(int i, int Budget){
        name = "Custom Pole " + i;
        id = i;
        if (Budget == -1){
            hook = Random.Range(0, 11);
            bait = Random.Range(0, 11);
            reel = Random.Range(0, 11);
            charm = Random.Range(0, 11);
            durability = 5 + Random.Range(0, 11);
            currDur = durability;
            spriteID = Random.Range(0,6);
        } else if (Budget == 0){
            durability = 5;
            levelA();
            currDur = durability;
            spriteID = Random.Range(0,6);
        }
        // TO IMPLEMENT
    }

    public void levelA(){
        int pts = 0;
        
        if (Random.value > 0.5f){
            pts = Random.Range(4,11);
            assignPointsToRandomStat(pts, true);
        }

        int secondaryRolls = Random.Range(2,5);
        while (secondaryRolls > 0){
            pts = Random.Range(1,5);
            assignPointsToRandZero(pts, false);
            secondaryRolls -= 1;
        }

        clampStats();
    }

    public void clampStats(){
        hook = Mathf.Min(10, hook);
        bait = Mathf.Min(10, bait);
        reel = Mathf.Min(10, reel);
        charm = Mathf.Min(10, charm);
        durability = Mathf.Min(15, durability);
    }

    public void assignPointsToRandZero(int pts, bool excludeDur){
        int zeroCount = 0;
        int ind = 0;
        int statsIncluded = 5;
        if (excludeDur) statsIncluded -= 1;
        while (ind < statsIncluded){
            if (stats[ind] == 0) zeroCount += 1;
            ind += 1;
        }
        if (zeroCount != 0){
            int randomSelect = Random.Range(0,zeroCount);
            ind = 0;
            while (ind < statsIncluded){
                if (stats[ind] == 0){
                    if (randomSelect == 0){
                        stats[ind] = pts;
                        ind = 5;
                    } else {
                        randomSelect -= 1;
                    }
                }
                ind += 1;
            }
        }    
    }

    public void assignPointsToRandomStat(int pts, bool excludeDur){
        int randomSelection = Random.Range(0,5);
        if (excludeDur){
            randomSelection = Random.Range(0,4);
        }
        switch(randomSelection){
            case 0:
                hook += pts;
                break;
            case 1:
                bait += pts;
                break;
            case 2:
                reel += pts;
                break;
            case 3:
                charm += pts;
                break;
            case 4:
                durability += pts;
                break;
        }
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
        spriteID = Random.Range(0,6);
    }

    public void addItem(){
        // TO IMPLEMENT
    }

    public bool weaken(int durLoss){
        currDur -= durLoss;
        return (currDur <= 0);
    }
}