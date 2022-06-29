using System.Collections.Generic;
using System.Linq;
using pow.aidkit;
using UnityEngine;

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
            Debug.Log($"[TestUsersHandler] {json}");
            var testUsers = JsonUtility.FromJson<TestDeviceList>(json);
            _testDevices = testUsers.testDevices.ToList();
            GetDeviceAdId();
        }

        private void GetDeviceAdId()
        {
            Application.RequestAdvertisingIdentifierAsync(
                (advertisingId, trackingEnabled, error) =>
                {
                    Debug.Log($"[TestDeviceHandler] advertisingId {advertisingId} , {trackingEnabled}, {error}");
                    if (_testDevices.Any(testDevice => testDevice.adID == advertisingId))
                    {
                        onTestDeviceFetched?.Invoke();
                    }
                }
            );
        }
    }
}