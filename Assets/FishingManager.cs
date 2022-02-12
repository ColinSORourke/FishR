using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;
using Unity.Notifications.iOS;

using UnityEngine;

public class FishingManager : MonoBehaviour
{
    public fishingStatus currFishing;
    public Button fishingButton;
    public Text statusText;
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
            currFishing = JsonUtility.FromJson<fishingStatusString>(JSON).backToStatus();
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
            if (DateTime.Now > currFishing.biteTime){
                if (DateTime.Now > currFishing.biteTime + currFishing.biteDuration){
                    statusText.GetComponent<Text>().text = "Missed a bite!";
                    currFishing.actuallyFishing = false;
                } else {
                    statusText.GetComponent<Text>().text = "There is currently a bite!";
                }
                timeText.GetComponent<Text>().text = "You caught something!";
                fishingButton.GetComponent<Button>().interactable = true;
            } else {
                TimeSpan diffTime = DateTime.Now - currFishing.startTime;
                string minTime;
                if (currFishing.minTime <= diffTime){
                    minTime = "Now";
                } else {
                    
                    minTime = (currFishing.minTime - diffTime).Minutes + ":" + (currFishing.minTime - diffTime).Seconds;
                }
                string maxTime = (currFishing.maxTime - diffTime).Minutes + ":" + (currFishing.maxTime - diffTime).Seconds;
                timeText.GetComponent<Text>().text = "Catch between " + minTime + "  and " + maxTime + " from Now";
            }
            
        }
    }

    public void startFishing(){
        statusText.GetComponent<Text>().text = "You are currently fishing!";
        currFishing = new fishingStatus();
        int Minutes = Random.Range(0,3);
        int Seconds = Random.Range(15,45);
        currFishing.actuallyFishing = true;
        currFishing.startTime = DateTime.Now;
        currFishing.biteTime = DateTime.Now + new TimeSpan(0, Minutes, Seconds);
        currFishing.biteDuration = new TimeSpan(0, 0, 20);
        currFishing.minTime = new TimeSpan(0, 0, 15);
        currFishing.maxTime = new TimeSpan(0, 2, 45);

        string json = JsonUtility.ToJson(currFishing.toStringObj());

        Debug.Log("Saving as JSON: " + json);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/FishingData.json", json);

        notifs.scheduleNotification(new TimeSpan(0, Minutes, Seconds), new TimeSpan(0, 0, 20));
    }

    

}

[Serializable]
public class fishingStatus{
    public bool actuallyFishing;
    public DateTime startTime;
    public DateTime biteTime;
    public TimeSpan biteDuration;
    public TimeSpan minTime;
    public TimeSpan maxTime;

    public fishingStatusString toStringObj(){
        fishingStatusString output = new fishingStatusString();
        output.actuallyFishing = this.actuallyFishing;
        output.startTime = this.startTime.ToString();
        output.biteTime = this.biteTime.ToString();
        output.minTime = this.minTime.ToString();
        output.maxTime = this.maxTime.ToString();
        output.biteDuration = this.biteDuration.ToString();
        return output;
    }
}

public class fishingStatusString{
    public bool actuallyFishing;
    public String startTime;
    public String biteTime;
    public String biteDuration;
    public String minTime;
    public String maxTime;

    public fishingStatus backToStatus(){
        fishingStatus output = new fishingStatus();
        output.actuallyFishing = this.actuallyFishing;
        output.startTime = DateTime.Parse(this.startTime);
        output.biteTime = DateTime.Parse(this.biteTime);
        output.minTime = TimeSpan.Parse(this.minTime);
        output.maxTime = TimeSpan.Parse(this.maxTime);
        output.biteDuration = TimeSpan.Parse(this.biteDuration);
        return output;
    }
}