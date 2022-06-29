using System.Collections.Generic;
using System.Linq;
using pow.aidkit;
using UnityEngine;

namespace pow.hermes
{
    [CreateAssetMenu(fileName = "TestUsersHandler", menuName = "POW_SDK/Hermes/TestUsersHandler", order = 0)]
    public class TestUsersHandler : ScriptableObject
    {
        [SerializeField] private GameEvent onTestUserFetched;
        private List<TestUser> _testUsers = new List<TestUser>();

        public List<TestUser> TestUsers => _testUsers;

        // Trigger this function from firebase remote config controller
        public void AddTestUsers(string json)
        {
            var testUsers = JsonUtility.FromJson<TestUsersList>(json);
            _testUsers = testUsers.testUsers.ToList();
            GetDeviceAdId();
        }

        private void GetDeviceAdId()
        {
            Application.RequestAdvertisingIdentifierAsync(
                (advertisingId, trackingEnabled, error) =>
                {
                    Debug.Log($"[TestUsersHandler] advertisingId {advertisingId} , {trackingEnabled}, {error}");
                    if (_testUsers.Any(testUser => testUser.adID == advertisingId))
                    {
                        onTestUserFetched?.Invoke();
                    }
                }
            );
        }
    }
}