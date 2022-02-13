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
    public int index;
    public string Description;
    public int unlockCost;
    public int hintCost;

    public Sprite background;
    public Sprite banner;

    public serialTimeSpan baseMinTime;
    public serialTimeSpan baseMaxTime;

    public serialTimeSpan minScalar;
    public serialTimeSpan maxScalar;

    public FishObj commonFish;
    public FishObj uncommonFish;
    public FishObj rareFish;
    public FishObj specialFish;

    public virtual bool specialCondition(){

        return true;
    }

    public TimeSpan minTime(int hook){
        return baseMinTime.deserialize() - (multiplyTime(minScalar.deserialize(), hook));
    }

    public serialTimeSpan serialMinTime(int hook){
        return new serialTimeSpan(minTime(hook));
    }

    public TimeSpan maxTime(int bait){
        return baseMaxTime.deserialize() - (multiplyTime(minScalar.deserialize(), bait));
    }

    public serialTimeSpan serialMaxTime(int bait){
        return new serialTimeSpan(maxTime(bait));
    }
    
    public TimeSpan multiplyTime(TimeSpan original, int scalar){
        return new TimeSpan(original.Hours * scalar, original.Minutes * scalar, original.Seconds * scalar);
    }

    public TimeSpan randomDuration(int hook, int bait){
        
        var minimum = minTime(hook);
        var maximum = maxTime(bait);
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

    public serialTimeSpan(TimeSpan original){
        hours = original.Hours;
        minutes = original.Minutes;
        seconds = original.Seconds;
    }

    public serialTimeSpan(int h, int m, int s){
        hours = h;
        minutes = m;
        seconds = s;
    }

    public TimeSpan deserialize(){
        return new TimeSpan(hours, minutes, seconds);
    }
}

[Serializable]
public class serialDateTime{
    public int year;
    public int month;
    public int day;
    public int hours;
    public int minutes;
    public int seconds;

    public serialDateTime(DateTime original){
        year = original.Year;
        month = original.Month;
        day = original.Day;
        hours = original.Hour;
        minutes = original.Minute;
        seconds = original.Second;
    }

    public serialDateTime(int y, int mon, int d, int h, int m, int s){
        year = y;
        month = mon;
        day = d;
        hours = h;
        minutes = m;
        seconds = s;
    }

    public DateTime deserialize(){
        return new DateTime(year, month, day, hours, minutes, seconds);
    }
}
