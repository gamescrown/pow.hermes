using System.Collections.Generic;
using com.adjust.sdk;
using Firebase.Analytics;
using GameAnalyticsSDK;
using UnityEngine;

namespace pow.hermes
{
    public static class EventSender
    {
        public static void LogAdjustEvent(string eventTokenValue)
        {
            var adjustEvent = new AdjustEvent(eventTokenValue);
            Adjust.trackEvent(adjustEvent);
            Debug.Log("Send event adjust token: " + eventTokenValue);
        }

        public static void LogFirebaseParametricEvent(string eventName, Parameter[] parameters)
        {
            if (!EventController.Instance.PoliciesIsAccepted || !FirebaseInit.Instance.isFirebaseInitialized)
            {
                EventController.Instance.OnPrivacyPoliciyAcceptedEventActions.Enqueue(() =>
                {
                    Debug.Log("Event Dequeue: " + eventName);
                    LogFirebaseParametricEvent(eventName, parameters);
                });
            }
            else
            {
                FirebaseAnalytics.LogEvent(
                    eventName,
                    parameters
                );
            }
        }

        public static void LogFirebaseEvent(string eventName)
        {
            if (!EventController.Instance.PoliciesIsAccepted || !FirebaseInit.Instance.isFirebaseInitialized)
            {
                EventController.Instance.OnPrivacyPoliciyAcceptedEventActions.Enqueue(() =>
                {
                    Debug.Log("Event Dequeue: " + eventName);
                    FirebaseAnalytics.LogEvent(eventName);
                });
            }
            else
            {
                FirebaseAnalytics.LogEvent(eventName);
            }
        }

        public static void GAParametricLevelStartEvent(string progression1, Dictionary<string, object> parameters)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, progression1, "Level_Progress", parameters);
        }

        public static void GAParametricLevelCompleteEvent(string progression1, int completeTime,
            Dictionary<string, object> parameters)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, progression1, "Level_Progress",
                completeTime, parameters); // with complete time
        }

        public static void GAParametricLevelFailEvent(string progression1, Dictionary<string, object> parameters)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, progression1, "Level_Progress", parameters);
        }

        public static void GAAdEvent(GAAdAction gaAdAction, GAAdType gaAdType, string adNetworkName, string adMedium)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewAdEvent(gaAdAction, gaAdType, adNetworkName, adMedium);
        }

        public static void GABusinessEvent(string currency, int amount, string itemType, string sku,
            string redirectFrom, Dictionary<string, object> parameters)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewBusinessEvent(currency, amount, itemType, sku, redirectFrom, parameters);
        }

        public static void GADesignEvent(string eventArg1, string eventArg2)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewDesignEvent($"{eventArg1}:{eventArg2}");
        }

        public static void GADesignEvent(string eventArg1, string eventArg2, Dictionary<string, object> fields)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewDesignEvent($"{eventArg1}:{eventArg2}", fields);
        }


        public static void GAResourceEvent(GAResourceFlowType flowType, string resourceCurrency, string itemType,
            string itemName, int amount)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewResourceEvent(flowType, resourceCurrency, amount, itemType, itemName);
        }

        public static void AdjustIAPEvents(string productIdentifierKey, float price, string transaction,
            string isoCurrencyCode)
        {
            Debug.Log(
                $"Sending total purchase analytics data to Adjust: {productIdentifierKey},{price},{transaction},{isoCurrencyCode}");
            //Adjust Purchase track
            AdjustEvent adjustEvent = new AdjustEvent(productIdentifierKey);
            adjustEvent.setRevenue((float) (0.7f * price), isoCurrencyCode);
            adjustEvent.setTransactionId(transaction);
            Adjust.trackEvent(adjustEvent);
        }

        public static void AdjustEcpmEvents(string token, double ecpm)
        {
            Debug.Log($"Sending eCPM data to Adjust: {token},{ecpm}");
            AdjustEvent adjustEvent = new AdjustEvent(token);

            adjustEvent.setRevenue(ecpm, "USD");
            Adjust.trackEvent(adjustEvent);
        }

        public static void SetUserPropertyForLevel(int level)
        {
            FirebaseAnalytics.SetUserProperty("level_id", $"level_{level:0000}");
            Debug.Log($"Set user level property successfully: level_{level:0000}");
        }

        public static void SetPurchaserUserProperty()
        {
            FirebaseAnalytics.SetUserProperty("purchaser_user", "true");
            Debug.Log($"Set user purchaser status property successfully: purchaser");
        }

        public static void SetNoAdsPurchaserUserProperty()
        {
            FirebaseAnalytics.SetUserProperty("no_ads", "true");
            Debug.Log($"Set user no ads purchaser status property successfully: no_ads_purchaser");
        }

        public static void SetUserProperty(string key, string value)
        {
            FirebaseAnalytics.SetUserProperty(key, value);
            Debug.Log($"Set user user property {key} successfully: {value}");
        }
    }
}