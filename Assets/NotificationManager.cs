using System.Collections;
using System.Collections.Generic;
using System;
using Random=UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.iOS;

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

    public virtual void scheduleNotification(TimeSpan fromNow, TimeSpan duration){
        //Code goes here
    }
}
