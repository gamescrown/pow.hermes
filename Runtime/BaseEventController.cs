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


        public void SendQueuedEvents()
        {
            Debug.Log($"{OnPrivacyPoliciyAcceptedEventActions.Count} Queued events sending...");
            while (OnPrivacyPoliciyAcceptedEventActions.Count > 0)
            {
                OnPrivacyPoliciyAcceptedEventActions.Dequeue().Invoke();
            }
        }
    }
}