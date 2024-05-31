using System.Collections.Generic;
using UnityEngine;

namespace DeiveEx.Utilities
{
    public static partial class UtilityServices
    {
        public static class GameObjectService
        {
            public static void DestroyAndClearGOList<T>(IList<T> listToClear) where T : Object
            {
                DestroyGOList(listToClear);
                listToClear.Clear();
            }
            
            public static void DestroyGOList<T>(IEnumerable<T> listToClear) where T : Object
            {
                foreach (var obj in listToClear)
                {
                    if(obj == null)
                        continue;
                    
                    switch (obj)
                    {
                        case Component component:
                            Object.Destroy(component.gameObject);
                            break;
                        
                        case GameObject gameObject:
                            Object.Destroy(gameObject);
                            break;
                        
                        default:
                            Debug.LogWarning($"Cannot destroy object of type {typeof(T)}");
                            break;
                    }
                }
            }
        }
    }
}
