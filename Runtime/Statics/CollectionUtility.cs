using System.Collections.Generic;
using System.Linq;

namespace DeiveEx.Utilities
{
    public static class CollectionUtility
    {
        public static bool IsNullOrEmpty<T>(IEnumerable<T> data) {
            return data == null || !data.Any();
        }
    }
}
