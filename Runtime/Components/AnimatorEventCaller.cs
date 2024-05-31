using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DeiveEx.Utilities
{
    public class AnimatorEventCaller : MonoBehaviour
    {
        [Serializable]
        public class AnimationEvent
        {
#if ODIN_INSPECTOR
            [TableColumnWidth(150, Resizable = false)]
#endif
            public string EventName;
            [SerializeReference]
            public List<UnityEvent> EventList;
        }
        
#if ODIN_INSPECTOR
        [TableList]
#endif
        [SerializeField] List<AnimationEvent> _events = new();

        public void CallEvent(string eventID)
        {
            var animEvent = _events.FirstOrDefault(x => x.EventName == eventID);
            
            if(animEvent == null)
            {
                Debug.LogWarning($"Event with ID \"{eventID}\" not found");
                return;
            }
            
            foreach (var e in animEvent.EventList)
            {
                e.Invoke();
            }
        }
    }
}
