using System.Collections.Generic;
using com.adjust.sdk;
using Firebase.Analytics;
using GameAnalyticsSDK;
using pow.aidkit;
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
            if (!FirebaseInit.Instance.isFirebaseInitialized)
            {
                BaseEventController.Instance.OnPrivacyPoliciyAcceptedEventActions.Enqueue(() =>
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
            if (!FirebaseInit.Instance.isFirebaseInitialized)
            {
                BaseEventController.Instance.OnPrivacyPoliciyAcceptedEventActions.Enqueue(() =>
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

        public static void LogFirebaseEvent(string eventName, string parameterName, string parameterValue)
        {
            if (!FirebaseInit.Instance.isFirebaseInitialized)
            {
                BaseEventController.Instance.OnPrivacyPoliciyAcceptedEventActions.Enqueue(() =>
                {
                    Debug.Log("Event Dequeue: " + eventName);
                    FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
                });
            }
            else
            {
                FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
            }
        }

        public static void LogFirebaseEvent(string eventName, string parameterName, int parameterValue)
        {
            if (!FirebaseInit.Instance.isFirebaseInitialized)
            {
                BaseEventController.Instance.OnPrivacyPoliciyAcceptedEventActions.Enqueue(() =>
                {
                    Debug.Log("Event Dequeue: " + eventName);
                    FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
                });
            }
            else
            {
                FirebaseAnalytics.LogEvent(eventName, parameterName, parameterValue);
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

        public static void GADesignEvent(string eventArg1, string eventArg2, Dictionary<string, object> fields)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewDesignEvent($"{eventArg1}:{eventArg2}", fields);
        }

        public static void GADesignEvent(string eventArg1, string eventArg2)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewDesignEvent($"{eventArg1}:{eventArg2}");
        }


        public static void GADesignEvent(string eventArg1)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            GameAnalytics.NewDesignEvent($"{eventArg1}");
        }

        public static void GAResourceEvent(GAResourceFlowType flowType, string resourceCurrency, string itemType,
            string itemName, int amount)
        {
            if (GAInit.InstanceExists && !GAInit.Instance.HasInitializeBeenCalled) return;
            var tmpResourceCurrencyList = resourceCurrency.Split('_');
            var tmpResourceCurrency = tmpResourceCurrencyList[0];
            for (int i = 1; i < tmpResourceCurrencyList.Length; i++)
            {
                tmpResourceCurrency += Converter.FirstCharToUpper(tmpResourceCurrencyList[i].Replace("_", ""));
            }

            var itemTypeList = itemType.Split('_');
            var itemTypeName = itemTypeList[0];
            for (int i = 1; i < itemTypeList.Length; i++)
            {
                itemTypeName += Converter.FirstCharToUpper(itemTypeList[i].Replace("_", ""));
            }

            GameAnalytics.NewResourceEvent(flowType, tmpResourceCurrency, amount, itemTypeName, itemName);
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

        public static void AdjustApplovinAdRevenueEvent(
            double revenue,
            string networkName,
            string adUnitIdentifier,
            string placement)
        {
            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);

            adjustAdRevenue.setRevenue(revenue, "USD");
            adjustAdRevenue.setAdRevenueNetwork(networkName);
            adjustAdRevenue.setAdRevenueUnit(adUnitIdentifier);
            adjustAdRevenue.setAdRevenuePlacement(placement);

            Adjust.trackAdRevenue(adjustAdRevenue);
        }

        public static void SetUserPropertyForLevel(int level)
        {
            SetUserProperty("level_id", $"level_{level:0000}");
        }

        public static void SetUserPropertyForMusic(bool isOn)
        {
            string s = isOn ? "on" : "off";
            SetUserProperty("music", s);
        }

        public static void SetPurchaserUserProperty()
        {
            SetUserProperty("purchaser_user", "true");
        }

        public static void SetNoAdsPurchaserUserProperty()
        {
            SetUserProperty("no_ads", "true");
        }

        public static void SetUserProperty(string key, string value)
        {
            FirebaseAnalytics.SetUserProperty(key, value);
            Debug.Log($"Set user user property {key} successfully: {value}");
        }
    }
}