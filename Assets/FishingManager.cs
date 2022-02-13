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
            Debug.Log(JSON);
            reader.Close();
            currFishing = JsonUtility.FromJson<fishingStatus>(JSON);
        } else {
            currFishing = new fishingStatus();
            currFishing.actuallyFishing = false;
        }

        InvokeRepeating("halfMinuteTick", 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            }
            if (TheNow > biteTime){
                if (TheNow > biteEnd){
                    currFishing.actuallyFishing = false;
                    catchPanel.transform.GetChild(1).GetComponent<CatchButton>().fail();
                    catchPanel.transform.GetChild(2).GetComponent<Text>().text = "Awwwww, you missed the fish. Better luck next time.";
                } else {
                    TimeSpan remainingTime = currFishing.biteDuration.deserialize() - (TheNow - biteTime);
                    catchPanel.transform.GetChild(2).GetComponent<Text>().text = "You got a bite! You have " + remainingTime.Minutes + ":" + remainingTime.Seconds + " left to catch it.";
                }
                if (!catchPanel.activeInHierarchy){
                    catchPanel.SetActive(true);
                }
            } else {
                TimeSpan diffTime = TheNow - startTime;
                string minString;
                if (minTime <= diffTime){
                    minString = "Now";
                } else {
                    minString = (minTime - diffTime).Minutes + ":" + (minTime - diffTime).Seconds;
                }
                string maxString = (maxTime - diffTime).Minutes + ":" + (maxTime - diffTime).Seconds;
                timeText.GetComponent<Text>().text = "Catch between " + minString + "  and " + maxString + " from Now";
            }
        }
    }

    public void startFishing(){

        Zone currZone = this.GetComponent<PlayerData>().currZone;
        TimeSpan timeTillBite = currZone.randomDuration(0, 0);

        currFishing = new fishingStatus();
        currFishing.actuallyFishing = true;
        currFishing.startTime = new serialDateTime(DateTime.Now);
        currFishing.biteTime = new serialDateTime(DateTime.Now + timeTillBite);
        currFishing.biteDuration = new serialTimeSpan(0, 0, 20);
        currFishing.minTime = currZone.serialMinTime(0);
        currFishing.maxTime = currZone.serialMaxTime(0);

        currFishing.missID = notifs.scheduleNotification(timeTillBite, new TimeSpan(0, 0, 20));
        saveFishing();
    }

    public void stopFishing(){
        currFishing.actuallyFishing = false;
        saveFishing();
        notifs.unscheduleMiss(currFishing.missID);
    }

    public void saveFishing(){
        string json = JsonUtility.ToJson(currFishing);
        Debug.Log("Saving as JSON: " + json);
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