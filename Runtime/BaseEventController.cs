using System;
using System.Collections.Generic;
using System.Reflection;
using com.adjust.sdk;
using Firebase.Analytics;
using GameAnalyticsSDK;
using pow.aidkit;
using UnityEngine;

namespace pow.hermes
{
    public class BaseEventController : Singleton<BaseEventController>
    {
        [SerializeField] private Policies policies;


        public bool PoliciesIsAccepted => policies.IsAccepted;
        public Queue<Action> OnPrivacyPoliciyAcceptedEventActions = new Queue<Action>();

        private readonly string IsGivenKey = "is_given";

        public void SendQueuedEvents()
        {
            while (OnPrivacyPoliciyAcceptedEventActions.Count > 0)
            {
                Debug.Log($"{OnPrivacyPoliciyAcceptedEventActions.Count} Queued events sending...");
                OnPrivacyPoliciyAcceptedEventActions.Dequeue().Invoke();
            }
        }

        // Send Notification Permission event in only IOS region, so can not show usage
        // EventController instance is not exist when this function called.
        // If this event is necessary, change location of Analytics Initialization and EventSender scripts
        public void SendNotificationPermissionViewEvent()
        {
            EventSender.LogFirebaseEvent("notification_permission_view");
        }

        // Send Notification Permission event in only IOS region, so can not show usage
        // EventController instance is not exist when this function called.
        // If this event is necessary, change location of Analytics Initialization and EventSender scripts
        public void SendNotificationPermissionPassEvent(bool isGiven)
        {
            EventSender.LogFirebaseParametricEvent(
                "notification_permission_pass",
                new Parameter[]
                {
                    new Parameter(IsGivenKey, isGiven.ToString())
                }
            );
        }
    }
}