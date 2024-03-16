using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DeiveEx.Utilities
{
    public class CollectionUtility
    {
        public static bool IsNullOrEmpty<T>(IEnumerable<T> data) {
            return data == null || !data.Any();
        }
    }
}
