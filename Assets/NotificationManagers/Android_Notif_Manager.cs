#if UNITY_EDITOR || UNITY_ANDROID

using System.Collections;
using System.Collections.Generic;
using System;
using Random=UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;

public class Android_Notif_Manager : NotificationManager
{
    AndroidNotificationChannel myChannel;

    // Start is called before the first frame update
    void Start()
    {
        myChannel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(myChannel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string scheduleNotification(TimeSpan fromNow, TimeSpan duration, int id){
        var notificationA = new AndroidNotification();
        notificationA.Title = "A Bite!";
        notificationA.Text = "You caught something! Check in in the next " + duration.Minutes + " minutes to claim it!";
        notificationA.ShouldAutoCancel = true;
        notificationA.FireTime = System.DateTime.Now + fromNow;

        AndroidNotificationCenter.SendNotification(notificationA, myChannel.Id);

        var notificationB = new AndroidNotification();
        notificationB.Title = "Too Slow!";
        notificationB.Text = "You caught something, but it got away! Better luck next time.";
        notificationB.ShouldAutoCancel = true;
        notificationB.FireTime = System.DateTime.Now + fromNow + duration;

        var missID = Convert.ToString( AndroidNotificationCenter.SendNotification(notificationB, myChannel.Id) );
        return missID;
    }

    public override void unscheduleMiss(string id){
        AndroidNotificationCenter.CancelNotification( Int32.Parse(id) );
    }
}

#endif
