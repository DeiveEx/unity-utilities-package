using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public string GetScenePath(Transform transform)
        {
            var current = transform;
            var inScenePath = new List<string> { current.name };
            
            while (current != transform.root)
            {
                current = current.parent;
                inScenePath.Add(current.name);
            }
            
            var sb = new StringBuilder(transform.gameObject.scene.name);
            
            foreach (var item in Enumerable.Reverse(inScenePath))
            {
                sb.Append($"\\{item}");
            }
            
            return sb.ToString().TrimStart('\\');
        }
    }
}
