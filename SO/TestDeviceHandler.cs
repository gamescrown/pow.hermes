using System.Collections.Generic;
using System.Linq;
using pow.aidkit;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

namespace pow.hermes
{
    [CreateAssetMenu(fileName = "TestDeviceHandler", menuName = "POW_SDK/Hermes/TestDeviceHandler", order = 0)]
    public class TestDeviceHandler : ScriptableObject
    {
        [SerializeField] private GameEvent onTestDeviceFetched;
        private List<TestDevice> _testDevices = new List<TestDevice>();
        private string _adId;

        public string AdId => _adId;

        // Trigger this function from firebase remote config controller
        public void AddTestUsers(string json)
        {
            Debug.Log($"[TestDeviceHandler] {json}");
            var testUsers = JsonUtility.FromJson<TestDeviceList>(json);
            _testDevices = testUsers.testDevices.ToList();
#if UNITY_EDITOR
#elif UNITY_ANDROID
            GetDeviceAdIdOnAndroid();
#elif UNITY_IOS
            GetDeviceAdIdOnIOS();
#endif
        }

        private void GetDeviceAdIdOnAndroid()
        {
            Debug.Log($"[TestDeviceHandler] RequestAdvertisingIdentifierAsync begin...");
            //Debug.Log($"[TestDeviceHandler] advertisingId {Device.advertisingIdentifier}");
            _adId = AdvertisementIdController.Instance.GetAdvertisingId();
            Debug.Log($"[TestDeviceHandler] adID {_adId}");

            if (_testDevices.Any(testDevice => testDevice.adID == _adId))
            {
                Debug.Log($"[TestDeviceHandler] Ad Id found on list");
                onTestDeviceFetched?.Invoke();
                Debug.Log($"[TestDeviceHandler] Ad Id found on list action invoked");
            }

            Debug.Log($"[TestDeviceHandler] RequestAdvertisingIdentifierAsync end...");
        }

        public void GetDeviceAdIdOnIOS()
        {
#if UNITY_IOS
            Debug.Log($"[TestDeviceHandler] RequestAdvertisingIdentifierAsync begin...");
            _adId = Device.advertisingIdentifier;
            Debug.Log($"[TestDeviceHandler] adID {_adId}");

            if (_testDevices.Any(testDevice => testDevice.adID == _adId))
            {
                Debug.Log($"[TestDeviceHandler] Ad Id found on list");
                onTestDeviceFetched?.Invoke();
                Debug.Log($"[TestDeviceHandler] Ad Id found on list action invoked");
            }

            Debug.Log($"[TestDeviceHandler] RequestAdvertisingIdentifierAsync end...");
#endif
        }
    }
}