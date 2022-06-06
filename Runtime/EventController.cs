using System;
using System.Collections.Generic;
using pow.aidkit;
using UnityEngine;

namespace pow.hermes
{
    public class EventController : Singleton<EventController>
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