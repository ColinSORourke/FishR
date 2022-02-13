using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Zone", menuName = "ScriptableObjects/Zone", order = 1)]
public class Zone : ScriptableObject
{
    public string zoneName;
    public string Description;
    public int unlockCost;
    public int hintCost;

    public Sprite background;
    public Sprite banner;

    public serialTimeSpan minTime;
    public serialTimeSpan maxTime;

    public FishObj commonFish;
    public FishObj uncommonFish;
    public FishObj rareFish;
    public FishObj specialFish;

    public virtual bool specialCondition(){

        return true;
    }

    public TimeSpan randomDuration(){
        var minimum = minTime.deserialize();
        var maximum = maxTime.deserialize();
        int hours = Random.Range(minimum.Hours, maximum.Hours);
        int minutes = Random.Range(minimum.Minutes, maximum.Minutes);
        int seconds = Random.Range(minimum.Seconds, maximum.Seconds);

        return new TimeSpan(hours, minutes, seconds);
    }

}

[Serializable]
public class serialTimeSpan{
    public int hours;
    public int minutes;
    public int seconds;

    public TimeSpan deserialize(){
        return new TimeSpan(hours, minutes, seconds);
    }
}
