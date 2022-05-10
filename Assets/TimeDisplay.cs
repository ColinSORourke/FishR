using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random=UnityEngine.Random;

public class TimeDisplay : MonoBehaviour
{
    public void displayFutureTime(Zone z, int h, int b){
        var min = z.minTime(h);
        var max = z.maxTime(b);
        if (min.Minutes == 0 && max.Minutes == 0){
            this.GetComponent<Text>().text = min.Hours + " to " + max.Hours + " hours";
        } else {
            this.GetComponent<Text>().text = min.Hours + ":" + (min.Minutes).ToString("00") + " to " + max.Hours + ":" + (max.Minutes).ToString("00") + " hours";
        }
    }

    public void displayRemainingTime(fishingStatus fs){
        TimeSpan diffTime = DateTime.Now - fs.startTime.deserialize();
        TimeSpan maxTime = fs.maxTime.deserialize();
        TimeSpan minTime = fs.minTime.deserialize();
        int roundMax = (minTime - diffTime).Seconds != 0 ? 1 : 0;
        int roundMin = (minTime - diffTime).Seconds != 0 ? 1 : 0;

        string minString;
        if (minTime <= diffTime){
            minString = "0";
        } else {
            minString = (minTime - diffTime).Hours + ":" + ((minTime - diffTime).Minutes + roundMin).ToString("00");
        }
        string maxString = (maxTime - diffTime).Hours + ":" + ((maxTime - diffTime).Minutes + roundMax).ToString("00");
        this.GetComponent<Text>().text = "Bite in " + minString + " to " + maxString + " hours from now";
    }

    public void bite(){
        this.GetComponent<Text>().text = "Got a bite!";
    }
}
