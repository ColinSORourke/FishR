using System.Collections;
using System.Collections.Generic;
using System;
using Random=UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual string scheduleNotification(TimeSpan fromNow, TimeSpan duration){
        //Code goes here
        return "BAD";
    }

    public virtual void unscheduleMiss(string id){

    }
}
