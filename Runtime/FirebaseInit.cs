using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using pow.aidkit;
using UnityEngine;

namespace pow.hermes
{
    public class FirebaseInit : Singleton<FirebaseInit>
    {
        public bool isFirebaseInitialized;
        public bool isRemoteConfigInitialized;

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                DependencyStatus dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    // Create and hold a reference to your FirebaseApp,
                    // where app is a Firebase.FirebaseApp property of your application class.
                    // Crashlytics will use the DefaultInstance, as well;
                    // this ensures that Crashlytics is initialized.
                    var app = FirebaseApp.DefaultInstance;
                    Debug.Log("Firebase initialized");
                    InitFirebaseRemoteConfig();
                    // Set a flag here for indicating that your project is ready to use Firebase.
                    //DatabaseHandler.Instance.GetLeaderBoardDatas();
                    isFirebaseInitialized = true;
                }
                else
                {
                    Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                    // Firebase Unity SDK is not safe to use here.
                }
            });
        }

        public void SetCollectionEnabled(bool enable)
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(enable);
            BaseEventController.Instance.SendQueuedEvents();
        }

        private void InitFirebaseRemoteConfig()
        {
            var defaults = new Dictionary<string, object>();
            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
                .ContinueWithOnMainThread(task =>
                {
                    Debug.Log("RemoteConfig configured and ready!");
                    FetchDataAsync();
                });
        }

        private Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            return fetchTask.ContinueWithOnMainThread(FetchComplete);
        }

        private void FetchComplete(Task fetchTask)
        {
            if (fetchTask.IsCanceled)
                Debug.Log("Fetch canceled.");
            else if (fetchTask.IsFaulted)
                Debug.Log("Fetch encountered an error.");
            else if (fetchTask.IsCompleted) Debug.Log("Fetch completed successfully!");

            ConfigInfo remoteConfigInfo = FirebaseRemoteConfig.DefaultInstance.Info;
            switch (remoteConfigInfo.LastFetchStatus)
            {
                case LastFetchStatus.Success:
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                        .ContinueWithOnMainThread(task =>
                        {
                            isRemoteConfigInitialized = true;
                            Debug.Log($"Remote data loaded and ready (last fetch time {remoteConfigInfo.FetchTime}).");
                        });

                    break;
                case LastFetchStatus.Failure:
                    switch (remoteConfigInfo.LastFetchFailureReason)
                    {
                        case FetchFailureReason.Error:
                            Debug.Log("Fetch failed for unknown reason");
                            break;
                        case FetchFailureReason.Throttled:
                            Debug.Log("Fetch throttled until " + remoteConfigInfo.ThrottledEndTime);
                            break;
                        case FetchFailureReason.Invalid:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                case LastFetchStatus.Pending:
                    Debug.Log("Latest Fetch call still pending.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public string GetString(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
        }

        public long GetLong(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key).LongValue;
        }

        public bool GetBool(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
        }

        public double GetDouble(string key)
        {
            return FirebaseRemoteConfig.DefaultInstance.GetValue(key).DoubleValue;
        }
    }
}