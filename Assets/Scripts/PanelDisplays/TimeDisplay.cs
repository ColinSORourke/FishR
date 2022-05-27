using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random=UnityEngine.Random;

public class TimeDisplay : MonoBehaviour
{
    public void displayFutureTime(Zone z, int h, int b){
        var min = z.minTime(b);
        var max = z.maxTime(h);
        if (min.Minutes == 0 && max.Minutes == 0){
            this.GetComponent<Text>().text = min.Hours + " to " + max.Hours + " hours";
        } else if (min.Hours == 0 && max.Hours == 0){
            this.GetComponent<Text>().text = min.Minutes + " to " + max.Minutes + " minutes";
        } else if (min.Hours == 0 && max.Hours == 1 && max.Minutes == 0){
            this.GetComponent<Text>().text = min.Minutes + " to 60 minutes";
        } else {
            this.GetComponent<Text>().text = min.Hours + ":" + (min.Minutes).ToString("00") + " to " + max.Hours + ":" + (max.Minutes).ToString("00") + " hours";
        }
    }

    public void displayRemainingTime(fishingStatus fs){
        TimeSpan diffTime = DateTime.Now - fs.startTime.deserialize();
        TimeSpan maxTime = fs.maxTime.deserialize();
        TimeSpan minTime = fs.minTime.deserialize();
        TimeSpan minRemain = minTime - diffTime;
        TimeSpan maxRemain = maxTime - diffTime;
        int roundMax = minRemain.Seconds != 0 ? 1 : 0;
        int roundMin = maxRemain.Seconds != 0 ? 1 : 0;

        string minString;
        string maxString = maxRemain.Hours + ":" + (maxRemain.Minutes + roundMax).ToString("00") + " hours";
        if (minTime <= diffTime){
            minString = "next ";
            if (maxRemain.Hours == 0){
                maxString = (maxRemain.Minutes + roundMax) + " minutes";
            }
        } else if (minRemain.Hours == 0 && maxRemain.Hours == 0){
            minString = (minRemain.Minutes + roundMin) + " to ";
            maxString = (maxRemain.Minutes + roundMax) + " minutes";
        } 
        else {
            minString = minRemain.Hours + ":" + (minRemain.Minutes + roundMin).ToString("00");
        }
        
        this.GetComponent<Text>().text = "Bite in " + minString + maxString + " from now";
    }

    public void bite(){
        this.GetComponent<Text>().text = "Got a bite!";
    }
}
