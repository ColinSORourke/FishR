#if UNITY_EDITOR || Unity_IOS

using System.Collections;
using System.Collections.Generic;
using System;
using Random=UnityEngine.Random;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.iOS;

public class iOS_Notif_Manager : NotificationManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string scheduleNotification(TimeSpan fromNow, TimeSpan duration){
        var timeTriggerA = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = fromNow,
            Repeats = false
        };

        var notificationA = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "fishingCaughtA",
            Title = "A Bite!",
            Body = "You caught something! Check in in the next " + duration.Minutes + " minutes to claim it!",
            Subtitle = "N/A",
            ShowInForeground = false,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTriggerA,
        };

        iOSNotificationCenter.ScheduleNotification(notificationA);

        var timeTriggerB = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = fromNow + duration,
            Repeats = false
        };

        var notificationB = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            Identifier = "fishingMissedA",
            Title = "Too Slow!",
            Body = "You caught something, but it got away! Better luck next time.",
            Subtitle = "N/A",
            ShowInForeground = false,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = timeTriggerB,
        };

        iOSNotificationCenter.ScheduleNotification(notificationB);
        return notificationB.Identifier;
    }

    public override void unscheduleMiss(string id){
        iOSNotificationCenter.RemoveScheduledNotification(id);
    }
}

#endif