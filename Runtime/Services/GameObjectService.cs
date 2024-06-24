using System.Collections.Generic;
using UnityEngine;

namespace DeiveEx.Utilities
{
    public class GameObjectService
    {
        public void DestroyAndClearGOList<T>(IList<T> listToClear) where T : Object
        {
            DestroyGOList(listToClear);
            listToClear.Clear();
        }
            
        public void DestroyGOList<T>(IEnumerable<T> listToClear) where T : Object
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
