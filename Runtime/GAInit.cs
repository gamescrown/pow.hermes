using GameAnalyticsSDK;
using pow.aidkit;
using UnityEngine;

namespace pow.hermes
{
    public class GAInit : Singleton<GAInit>
    {
        [SerializeField] private bool hasInitializeBeenCalled;

        public bool HasInitializeBeenCalled
        {
            get => hasInitializeBeenCalled;
            set => hasInitializeBeenCalled = value;
        }

        private void Start()
        {
            GameAnalytics.Initialize();
            HasInitializeBeenCalled = true;
        }
    }
}