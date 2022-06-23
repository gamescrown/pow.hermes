using System.Collections.Generic;
using UnityEngine;

namespace pow.hermes
{
    [CreateAssetMenu(fileName = "AdjustEventHandler", menuName = "POW_SDK/Hermes/AdjustEventHandler", order = 0)]
    public class AdjustEventHandler : ScriptableObject
    {
        private List<PowAdjustEvent> _adjustEvents = new List<PowAdjustEvent>();

        public void AddPowAdjustEvent(string key, string token)
        {
            _adjustEvents.Add(new PowAdjustEvent(key, token));
        }

        public string GetAdjustEventTokenByKey(string key)
        {
            PowAdjustEvent powAdjustEvent = _adjustEvents.Find(e => e._key == key);
            return powAdjustEvent == null ? string.Empty : _adjustEvents.Find(e => e._key == key)._token;
        }
    }
}