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
        [SerializeField] private AdjustEventHandler adjustEventHandler;


        public bool PoliciesIsAccepted => policies.IsAccepted;
        public Queue<Action> OnPrivacyPoliciyAcceptedEventActions = new Queue<Action>();

        private readonly string EcpmKey = "ecpm";
        private readonly string AdNetworkNameKey = "ad_network_name";
        private readonly string AdMediumKey = "ad_medium";


        public void SendQueuedEvents()
        {
            Debug.Log($"{OnPrivacyPoliciyAcceptedEventActions.Count} Queued events sending...");
            while (OnPrivacyPoliciyAcceptedEventActions.Count > 0)
            {
                OnPrivacyPoliciyAcceptedEventActions.Dequeue().Invoke();
            }
        }

        public void SendInterstitialAdEvent(
            GAAdAction gaAdAction,
            string interstitialAdStatus,
            string adNetworkName,
            string interstitialTag)
        {
            EventSender.GAAdEvent(gaAdAction, GAAdType.Interstitial, adNetworkName, interstitialTag);

            EventSender.LogFirebaseParametricEvent(
                interstitialAdStatus.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, interstitialTag)
                }
            );
        }

        public void SendInterstitialLoadEvent(string adNetworkName, string interstitialTag)
        {
            EventSender.GAAdEvent(GAAdAction.Loaded, GAAdType.Interstitial, adNetworkName, interstitialTag);

            EventSender.LogFirebaseParametricEvent(
                InterstitialAdStatus.interstitial_loaded.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, interstitialTag)
                }
            );
        }

        public void SendInterstitialShowEvent(string adNetworkName, string interstitialTag)
        {
            EventSender.GAAdEvent(GAAdAction.Show, GAAdType.Interstitial, adNetworkName, interstitialTag);

            EventSender.LogFirebaseParametricEvent(
                InterstitialAdStatus.interstitial_show.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, interstitialTag)
                }
            );
        }

        public void SendInterstitialFailedShowEvent(string adNetworkName, string interstitialTag)
        {
            EventSender.GAAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial, adNetworkName, interstitialTag);

            EventSender.LogFirebaseParametricEvent(
                InterstitialAdStatus.interstitial_failed_show.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, interstitialTag)
                }
            );
        }

        public void SendInterstitialClickedEvent(string adNetworkName, string interstitialTag)
        {
            EventSender.GAAdEvent(GAAdAction.Clicked, GAAdType.Interstitial, adNetworkName, interstitialTag);

            EventSender.LogFirebaseParametricEvent(
                InterstitialAdStatus.interstitial_click.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, interstitialTag)
                }
            );
        }

        public void SendCpmEvent(CpmKey cpmKey, double ecpm)
        {
            double revenueUsd = ecpm / (1000 * 100);
            EventSender.LogFirebaseParametricEvent(
                cpmKey.ToString(),
                new Parameter[]
                {
                    new Parameter(FirebaseAnalytics.ParameterCurrency, "USD"),
                    new Parameter(FirebaseAnalytics.ParameterValue, revenueUsd),
                    new Parameter(EcpmKey, ecpm)
                }
            );
            EventSender.AdjustEcpmEvents(adjustEventHandler.GetAjustEventTokenByKey(cpmKey.ToString()), revenueUsd);
        }
    }
}