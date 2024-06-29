using UnityEngine;

namespace DeiveEx.Utilities
{
    public static class ObjectExtensions
    {
        public static bool IsUnityNull(this object obj)
        {
            if (obj is Object unityObject)
                return unityObject == null;

            return obj == null;
        }
    }
}
