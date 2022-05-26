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
    public int durCost;

    public string hint;
    public string fullHint;

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

    public virtual bool specialCondition(fishingStatus status, FishingPole fp){

        return false;
    }

    public TimeSpan minTime(int hook){
        return baseMinTime.deserialize() - (multiplyTime(minScalar.deserialize(), hook));
    }

    public serialTimeSpan serialMinTime(int hook){
        return new serialTimeSpan(minTime(hook));
    }

    public TimeSpan maxTime(int bait){
        return baseMaxTime.deserialize() - (multiplyTime(maxScalar.deserialize(), bait));
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
        var minMinutes = minimum.Hours * 60 + minimum.Minutes;
        var maxMinutes = maximum.Hours * 60 + maximum.Minutes;
        var randMinutes = Random.Range(minMinutes, maxMinutes);
        int seconds = 30;

        return new TimeSpan(0, randMinutes, seconds);
    }

    public fishRarity catchFish(bool canSpecial, FishingPole fp, fishingStatus status){
        int luck = fp.charm;
        if ( canSpecial && specialCondition(status, fp) ){
            return fishRarity.special;
        } else {
            int myRand = Random.Range(0, 1001);
            if (myRand > (950 - (10 * luck))){
                return fishRarity.rare;
            } else if (myRand > (800 - (15 * luck) ) ){
                return fishRarity.uncommon;
            } else {
                return fishRarity.common;
            }
        }
    }

    public FishObj rarityCatch(fishRarity r){
        switch (r){
            case fishRarity.common:
                return commonFish;
            case fishRarity.uncommon:
                return uncommonFish;
            case fishRarity.rare:
                return rareFish;
            case fishRarity.special:
                return specialFish;
        }
        return commonFish;
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

    public serialDateTime(DateTime original, bool ignoreTOD = false){
        year = original.Year;
        month = original.Month;
        day = original.Day;
        if (!ignoreTOD){
            hours = original.Hour;
            minutes = original.Minute;
            seconds = original.Second;
        } 
    }

    public bool matchDate(serialDateTime other){
        return (year == other.year && month == other.month && day == other.day);
    }

    public int toSeed(){
        int result = year + (month * 100) + day;
        return result;
    }

    public serialDateTime(int y, int mon, int d, int h = 0, int m = 0, int s = 0){
        year = y;
        month = mon;
        day = d;
        hours = h;
        minutes = m;
        seconds = s;
    }

    public DateTime deserialize(bool ignoreTOD = false){
        if (!ignoreTOD){
            return new DateTime(year, month, day, hours, minutes, seconds);
        } else {
            return new DateTime(year, month, day, 0, 0, 0);
        }
        
    }
}
