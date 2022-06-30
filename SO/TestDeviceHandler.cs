using System.Collections.Generic;
using System.Linq;
using pow.aidkit;
using UnityEngine;

//using UnityEngine.iOS;

namespace pow.hermes
{
    [CreateAssetMenu(fileName = "TestDeviceHandler", menuName = "POW_SDK/Hermes/TestDeviceHandler", order = 0)]
    public class TestDeviceHandler : ScriptableObject
    {
        [SerializeField] private GameEvent onTestDeviceFetched;
        private List<TestDevice> _testDevices = new List<TestDevice>();

        // Trigger this function from firebase remote config controller
        public void AddTestUsers(string json)
        {
            Debug.Log($"[TestDeviceHandler] {json}");
            var testUsers = JsonUtility.FromJson<TestDeviceList>(json);
            _testDevices = testUsers.testDevices.ToList();
            GetDeviceAdId();
        }

        private void GetDeviceAdId()
        {
            Debug.Log($"[TestDeviceHandler] RequestAdvertisingIdentifierAsync begin...");
            //Debug.Log($"[TestDeviceHandler] advertisingId {Device.advertisingIdentifier}");
            var advertisingId = AdvertisementIdController.Instance.GetAdvertisingId();
            Debug.Log($"[TestDeviceHandler] adID {advertisingId}");

            if (_testDevices.Any(testDevice => testDevice.adID == advertisingId))
            {
                Debug.Log($"[TestDeviceHandler] Ad Id found on list");
                onTestDeviceFetched?.Invoke();
                Debug.Log($"[TestDeviceHandler] Ad Id found on list action invoked");
            }

            Debug.Log($"[TestDeviceHandler] RequestAdvertisingIdentifierAsync end...");
        }
    }
}