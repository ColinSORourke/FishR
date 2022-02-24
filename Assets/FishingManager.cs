using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public fishingStatus currFishing;
    public GameObject catchPanel;
    public Button fishingButton;
    public Text timeText;
    public NotificationManager notifs;

    // Start is called before the first frame update
    void Start()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/FishingData.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/FishingData.json"); 
            string JSON = reader.ReadToEnd();
            Debug.Log("Reading fish save JSON");
            reader.Close();
            currFishing = JsonUtility.FromJson<fishingStatus>(JSON);
        } else {
            currFishing = new fishingStatus();
            currFishing.actuallyFishing = false;
        }

        InvokeRepeating("halfMinuteTick", 0.0f, 30.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationPause(bool paused){
        if (!paused){
            halfMinuteTick();
        }
    }

    void halfMinuteTick(){
        if (currFishing.actuallyFishing){

            var TheNow = DateTime.Now;
            var startTime = currFishing.startTime.deserialize();
            var biteTime = currFishing.biteTime.deserialize();
            var biteEnd = currFishing.biteTime.deserialize() + currFishing.biteDuration.deserialize();
            var minTime = currFishing.minTime.deserialize();
            var maxTime = currFishing.maxTime.deserialize();


            if ( fishingButton.interactable){
                fishingButton.interactable = false;
                fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Currently Fishing!";
            }
            if (TheNow > biteTime){
                if (!catchPanel.activeInHierarchy){
                    catchPanel.SetActive(true);
                    this.GetComponent<PlayerData>().changeZone(currFishing.zoneIndex);
                    timeText.GetComponent<Text>().text = "Got a Bite!";
                }
                if (TheNow > biteEnd){
                    currFishing.actuallyFishing = false;
                    catchPanel.transform.GetChild(1).GetComponent<CatchButton>().fail();
                    catchPanel.transform.GetChild(2).GetComponent<Text>().text = "Awwwww, you missed the fish. Better luck next time.";
                    zoneTimeText(this.GetComponent<PlayerData>().currZone);
                } else {
                    TimeSpan remainingTime = currFishing.biteDuration.deserialize() - (TheNow - biteTime);
                    catchPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "You got a bite! You have " + remainingTime.Minutes + " minutes left to catch it.";
                }
                
            } else {
                TimeSpan diffTime = TheNow - startTime;
                string minString;
                if (minTime <= diffTime){
                    minString = "0";
                } else {
                    minString = (minTime - diffTime).Hours + ":" + ((minTime - diffTime).Minutes).ToString("00");
                }
                string maxString = (maxTime - diffTime).Hours + ":" + ((maxTime - diffTime).Minutes).ToString("00");
                timeText.GetComponent<Text>().text = "Bite in " + minString + " to " + maxString + " hours from now";
            }
        }
    }

    public void zoneTimeText(Zone currZone){
        if (!currFishing.actuallyFishing){
            var min = currZone.minTime(0);
            var max = currZone.maxTime(0);
            if (min.Minutes == 0 && max.Minutes == 0){
                timeText.GetComponent<Text>().text = min.Hours + " to " + max.Hours + " hours";
            } else {
                timeText.GetComponent<Text>().text = min.Hours + ":" + (min.Minutes).ToString("00") + " to " + max.Hours + ":" + (max.Minutes).ToString("00") + " hours";
            }
            
        }

    }

    public void startFishing(){
        Zone currZone = this.GetComponent<PlayerData>().currZone;
        TimeSpan timeTillBite = currZone.randomDuration(0, 0);

        currFishing = new fishingStatus();
        currFishing.actuallyFishing = true;
        currFishing.zoneIndex = this.GetComponent<PlayerData>().currZone.index;
        currFishing.startTime = new serialDateTime(DateTime.Now);
        currFishing.biteTime = new serialDateTime(DateTime.Now + timeTillBite);
        currFishing.biteDuration = new serialTimeSpan(0, 20, 0);
        currFishing.minTime = currZone.serialMinTime(0);
        currFishing.maxTime = currZone.serialMaxTime(0);

        fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Currently Fishing!";

        currFishing.missID = notifs.scheduleNotification(timeTillBite, new TimeSpan(0, 20, 0));
        saveFishing();
        halfMinuteTick();
    }

    public void stopFishing(){
        currFishing.actuallyFishing = false;
        saveFishing();
        notifs.unscheduleMiss(currFishing.missID);
        zoneTimeText(this.GetComponent<PlayerData>().currZone);
        fishingButton.transform.GetChild(0).GetComponent<Text>().text = "Tap to Fish!";
    }

    public void saveFishing(){
        string json = JsonUtility.ToJson(currFishing);
        Debug.Log("Saving Fishing Data");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/FishingData.json", json);
    }

}

[Serializable]
public class fishingStatus{
    public string missID;
    public int zoneIndex;
    public bool actuallyFishing;
    public serialDateTime startTime;
    public serialDateTime biteTime;
    public serialTimeSpan biteDuration;
    public serialTimeSpan minTime;
    public serialTimeSpan maxTime;
}