using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public fishingStatuses currFishing;
    public GameObject catchPanel;
    public Button fishingButton;
    public GameObject cancelButton;
    public TimeDisplay timeText;
    public NotificationManager notifs;

    void Awake(){
        if (System.IO.File.Exists(Application.persistentDataPath + "/FishingData.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/FishingData.json"); 
            string JSON = reader.ReadToEnd();
            Debug.Log("Reading fish save JSON");
            reader.Close();
            currFishing = JsonUtility.FromJson<fishingStatuses>(JSON);
        } else {
            currFishing = new fishingStatuses();
            saveFishing();
        }
    }

    public void increaseActive(){
        currFishing.maxActive += 1;
        saveFishing();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("halfMinuteTick", 0.0f, 30.0f);
    }

    void OnApplicationPause(bool paused){
        if (!paused){
            this.buttonUpdate();
            halfMinuteTick();
        }
    }

    public void halfMinuteTick(){
        Zone z = this.GetComponent<PlayerData>().currZone;
        fishingStatus fs = currFishing.activeInZone(z);
        if (fs != null){
            FishingPole fp = this.GetComponent<PoleManager>().getPoleByID(fs.poleID);
            this.GetComponent<PoleManager>().lockPole(fs.poleID);
            if ( fishingButton.interactable){
                fishingButton.interactable = false;
            }
            if (DateTime.Now > fs.biteTime.deserialize()){
                if (!catchPanel.activeInHierarchy){
                    catchPanel.SetActive(true);
                    timeText.bite();
                    fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Got a bite!";
                    cancelButton.SetActive(false);
                }

                if (DateTime.Now > fs.getAwayTime()){
                    catchPanel.transform.GetChild(0).GetComponent<CatchButton>().fail();
                    catchPanel.transform.GetChild(1).GetComponent<Text>().text = "Awwwww, you missed the fish. Better luck next time.";
                } else {
                    TimeSpan remainingTime = (fs.getAwayTime() - DateTime.Now);
                    int roundMin = remainingTime.Seconds != 0 ? 1 : 0;
                    catchPanel.transform.GetChild(1).GetComponent<Text>().text = "You got a bite! You have " + (remainingTime.Minutes + roundMin) + " minutes left to catch it.";
                }
            } else {
                fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Currently Fishing!";
                cancelButton.SetActive(true);
                timeText.displayRemainingTime(fs);
            }
        }
    }

    public void startFishing(){
        Zone z = this.GetComponent<PlayerData>().currZone;
        FishingPole fp = this.GetComponent<PoleManager>().getPole();
        if (currFishing.canFish()){
            int i = currFishing.startFishing(z, fp);
            this.GetComponent<PoleManager>().startUsing();
            fishingStatus fs = currFishing.activeInZone(z);
            fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Currently Fishing!";
            cancelButton.SetActive(true);
            string notifIDString = notifs.scheduleNotification(fs.timeTillBite(), fs.biteDuration.deserialize(), i);
            string[] notifIDs = notifIDString.Split('\n');
            currFishing.addIDs(z, notifIDs[0], notifIDs[1]);
            saveFishing();
            halfMinuteTick();
        }
    }

    public void stopFishing(){
        Zone z = this.GetComponent<PlayerData>().currZone;
        FishingPole fp = this.GetComponent<PoleManager>().getPole();
        fishingStatus fs = currFishing.activeInZone(z);
        currFishing.stopFishing(z);
        saveFishing();
        if (DateTime.Now < fs.biteTime.deserialize()){
            notifs.unscheduleMiss(fs.catchID);
        }
        if (DateTime.Now < fs.getAwayTime()){
            notifs.unscheduleMiss(fs.missID);
        }
        timeText.displayFutureTime(z, fp.hook, fp.bait);
        fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Tap to Fish!";
        fishingButton.interactable = true;
        cancelButton.SetActive(false);
        this.GetComponent<PoleManager>().stopUsing(fs.poleID);
        this.GetComponent<PoleManager>().unlockPole();
    }

    public void saveFishing(){
        string json = JsonUtility.ToJson(currFishing);
        Debug.Log("Saving Fishing Data");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/FishingData.json", json);
    }

    public void eraseData(){
        int i = 0;
        while (i < 3){
            fishingStatus fs = currFishing.statuses[i];
            if (fs.active){
                Debug.Log("Fishing Status " + i);
                if (DateTime.Now < fs.biteTime.deserialize()){
                    notifs.unscheduleMiss(fs.catchID);
                }
                if (DateTime.Now < fs.getAwayTime()){
                    notifs.unscheduleMiss(fs.missID);
                }
            }
            i += 1;
        }
        currFishing = new fishingStatuses();
        saveFishing();
    }

    public void buttonUpdateVars(Zone z, FishingPole fp, bool inUse){
        if (currFishing.activeInZone(z) == null){
            timeText.displayFutureTime(z, fp.hook, fp.bait);
            this.GetComponent<PoleManager>().unlockPole();
            if (currFishing.canFish()){
                fishingButton.interactable = !inUse;
                if (inUse){
                    fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Already Using that pole!";
                } else {
                    fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Tap to Fish!";
                }
            } else {
                fishingButton.interactable = false;
                fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Too much fishing!";
            }
            cancelButton.SetActive(false);
        }
    }

    public void buttonUpdate(){
        Zone z = this.GetComponent<PlayerData>().currZone;
        FishingPole fp = this.GetComponent<PoleManager>().getPole();
        bool inUse = this.GetComponent<PoleManager>().inUse();
        Debug.Log("Button Update poleID: " + fp.id + ", inUse: " + inUse);
        this.buttonUpdateVars(z, fp, inUse);
    }

    public void updateZone(Zone z){
        FishingPole fp = this.GetComponent<PoleManager>().getPole();
        bool inUse = this.GetComponent<PoleManager>().inUse();
        this.buttonUpdateVars(z, fp, inUse);
    }

    public void updatePole(FishingPole fp, bool inUse = false){
        Zone z = this.GetComponent<PlayerData>().currZone;
        Debug.Log("Pole Update poleID: " + fp.id + ", inUse: " + inUse);
        this.buttonUpdateVars(z, fp, inUse);
    }
}

[Serializable]
public class fishingStatuses{
    public fishingStatus[] statuses = new fishingStatus[3];
    public int activeNum = 0;
    public int maxActive = 1;

    public fishingStatuses(){
        statuses[0] = new fishingStatus();
        statuses[1] = new fishingStatus();
        statuses[2] = new fishingStatus();
    }

    public bool canFish(){
        return (activeNum < maxActive);
    }

    public int startFishing(Zone z, FishingPole fp){
        if (activeNum < maxActive){
            activeNum += 1;
            TimeSpan timeTillBite = z.randomDuration(fp.hook, fp.bait);

            fishingStatus fs = new fishingStatus();
            fs.active = true;
            fs.poleID = fp.id;
            fs.zoneIndex = z.index;
            fs.startTime = new serialDateTime(DateTime.Now);
            fs.biteTime = new serialDateTime(DateTime.Now + timeTillBite);
            fs.biteDuration = new serialTimeSpan(0, 20 + (fp.reel * 4), 0);
            fs.minTime = z.serialMinTime(fp.bait);
            fs.maxTime = z.serialMaxTime(fp.hook);

            int i = 0;
            while (i < 3){
                if (!statuses[i].active){
                    statuses[i] = fs;
                    return i;
                }
                i += 1;
            }
            return -1;
        } else {
            return -1;
        }
    }

    public void addIDs(Zone z, string a, string b){
        int zoneInd = z.index;
        int i = 0;
        while (i < 3){
            if (statuses[i].zoneIndex == zoneInd){
                statuses[i].catchID = a;
                statuses[i].missID = b;
                i = 3;
            }
            i += 1;
        }
    }

    public void stopFishing(Zone z){
        int zoneInd = z.index;
        int i = 0;
        while (i < 3){
            if (statuses[i].zoneIndex == zoneInd){
                statuses[i].active = false;
                i = 3;
            }
            i += 1;
        }
        activeNum -= 1;
    }

    public fishingStatus activeInZone(Zone z){
        int zoneInd = z.index;
        int i = 0;
        while (i < 3){
            if (statuses[i].zoneIndex == zoneInd && statuses[i].active){
                return statuses[i];
            }
            i += 1;
        }
        return null;
    }


}

[Serializable]
public class fishingStatus{
    public string catchID;
    public string missID;
    public int poleID;
    public int zoneIndex;
    public bool active;
    public serialDateTime startTime;
    public serialDateTime biteTime;
    public serialTimeSpan biteDuration;
    public serialTimeSpan minTime;
    public serialTimeSpan maxTime;

    public fishingStatus(){
        active = false;
    }

    public DateTime getAwayTime(){
        return biteTime.deserialize() + biteDuration.deserialize();
    }

    public TimeSpan timeTillBite(){
        return biteTime.deserialize() - DateTime.Now;
    }
}