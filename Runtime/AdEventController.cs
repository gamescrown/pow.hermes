using Firebase.Analytics;
using GameAnalyticsSDK;
using pow.aidkit;
using UnityEngine;

namespace pow.hermes
{
    public class AdEventController : Singleton<AdEventController>
    {
        [SerializeField] private AdjustEventHandler adjustEventHandler;

        private readonly int ecpm10 = 1000;
        private readonly int ecpm100 = 10000;
        private readonly int ecpm500 = 50000;
        private readonly int ecpm1000 = 100000;
        private readonly int ecpm2500 = 250000;

        private readonly string AdNetworkNameKey = "ad_network_name";
        private readonly string AdMediumKey = "ad_medium";
        private readonly string EcpmKey = "ecpm";

        #region CPM Events

        public void SendEcpmEvent(double ecpm)
        {
            if (ecpm < ecpm10)
            {
                SendCpmEvent(CpmKey.newCPMlessthan10, ecpm);
            }

            if (ecpm >= ecpm10 && ecpm < ecpm2500)
            {
                SendCpmEvent(CpmKey.newCPMgreaterthan10, ecpm);
            }

            if (ecpm >= ecpm100 && ecpm < ecpm2500)
            {
                SendCpmEvent(CpmKey.newCPMgreaterthan100, ecpm);
            }

            if (ecpm >= ecpm500 && ecpm < ecpm2500)
            {
                SendCpmEvent(CpmKey.newCPMgreaterthan500, ecpm);
            }

            if (ecpm >= ecpm1000 && ecpm < ecpm2500)
            {
                SendCpmEvent(CpmKey.newCPMgreaterthan1000, ecpm);
            }
        }

        public void SendEcpmEventExcludeBanner(double ecpm)
        {
            if (ecpm < ecpm2500)
            {
                SendCpmEvent(CpmKey.AdRevenue_Total, ecpm);
            }
        }

        public void SendEcpmEventOnlyRewarded(double ecpm)
        {
            if (ecpm > ecpm10 && ecpm < ecpm2500)
            {
                SendCpmEvent(CpmKey.CPM10_rewardedad, ecpm);
            }
        }

        private void SendCpmEvent(CpmKey cpmKey, double ecpm)
        {
            double revenueUsd = ecpm / (1000 * 100);
            //EventSender.LogFirebaseParametricEvent(
            //    cpmKey.ToString(),
            //    new Parameter[]
            //    {
            //        new Parameter(FirebaseAnalytics.ParameterCurrency, "USD"),
            //        new Parameter(FirebaseAnalytics.ParameterValue, revenueUsd),
            //        new Parameter(EcpmKey, ecpm)
            //    }
            //);
            EventSender.AdjustEcpmEvents(adjustEventHandler.GetAjustEventTokenByKey(cpmKey.ToString()), revenueUsd);
        }

        #endregion

        #region Interstitial Events

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

        #endregion

        #region Rewarded Ad Events

        public void SendRewardedVideoLoadedEvent(string adNetworkName, RewardedVideoTag rewardedVideoTag)
        {
            EventSender.GAAdEvent(GAAdAction.Loaded, GAAdType.RewardedVideo, adNetworkName,
                rewardedVideoTag.ToString());

            EventSender.LogFirebaseParametricEvent(
                RewardedVideoStatus.rewarded_ad_loaded.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, rewardedVideoTag.ToString())
                }
            );
        }

        public void SendRewardedVideoDisplayedEvent(string adNetworkName, string rewardedVideoTag)
        {
            EventSender.GAAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, adNetworkName, rewardedVideoTag);

            EventSender.LogFirebaseParametricEvent(
                RewardedVideoStatus.rewarded_ad_show.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, rewardedVideoTag)
                }
            );
        }

        public void SendRewardedVideoFailedShowEvent(string adNetworkName, string rewardedVideoTag)
        {
            EventSender.GAAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo, adNetworkName, rewardedVideoTag);

            EventSender.LogFirebaseParametricEvent(
                RewardedVideoStatus.rewarded_ad_failed_show.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, rewardedVideoTag)
                }
            );
        }

        public void SendRewardedVideoClickedEvent(string adNetworkName, string rewardedVideoTag)
        {
            EventSender.GAAdEvent(GAAdAction.Clicked, GAAdType.RewardedVideo, adNetworkName, rewardedVideoTag);

            EventSender.LogFirebaseParametricEvent(
                RewardedVideoStatus.rewarded_ad_click.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, rewardedVideoTag)
                }
            );
        }

        public void SendRewardedVideoReceivedRewardEvent(string adNetworkName, string rewardedVideoTag)
        {
            EventSender.GAAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, adNetworkName, rewardedVideoTag);

            EventSender.LogFirebaseParametricEvent(
                RewardedVideoStatus.rewarded_ad_complete.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName),
                    new Parameter(AdMediumKey, rewardedVideoTag)
                }
            );
        }

        #endregion

        #region Banner Ad Events

        public void SendBannerLoadedEvent(string adNetworkName)
        {
            EventSender.GAAdEvent(GAAdAction.Loaded, GAAdType.Banner, adNetworkName, "untagged");

            EventSender.LogFirebaseParametricEvent(
                BannerAdStatus.banner_loaded.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName)
                }
            );
        }

        public void SendBannerClickedEvent(string adNetworkName)
        {
            EventSender.GAAdEvent(GAAdAction.Clicked, GAAdType.Banner, adNetworkName, "untagged");

            EventSender.LogFirebaseParametricEvent(
                BannerAdStatus.banner_click.ToString(),
                new Parameter[]
                {
                    new Parameter(AdNetworkNameKey, adNetworkName)
                }
            );
        }

        #endregion
    }
}