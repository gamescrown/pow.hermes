using System.Collections.Generic;
using UnityEngine;

namespace pow.hermes
{
    [CreateAssetMenu(fileName = "AdjustEventHandler", menuName = "POW_SDK/Hermes/AdjustEventHandler", order = 0)]
    public class AdjustEventHandler : ScriptableObject
    {
        private List<PowAdjustEvent> adjustEvents = new List<PowAdjustEvent>();

        public string GetAjustEventTokenByKey(string key)
        {
            PowAdjustEvent powAdjustEvent = adjustEvents.Find(e => e._key == key);
            return powAdjustEvent == null ? string.Empty : adjustEvents.Find(e => e._key == key)._token;
        }
    }
}