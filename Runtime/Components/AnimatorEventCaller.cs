#if ODIN_INSPECTOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;

namespace DeiveEx.Utilities
{
    public class AnimatorEventCaller : SerializedMonoBehaviour
    {
        [SerializeField] Dictionary<string, List<UnityEvent>> _events = new();

        public void CallEvent(string eventID)
        {
            if (!_events.TryGetValue(eventID, out var eventList))
            {
                Debug.LogWarning($"Event with ID \"{eventID}\" not found");
                return;
            }

            foreach (var e in eventList)
            {
                e.Invoke();
            }
        }
    }
}
#endif
