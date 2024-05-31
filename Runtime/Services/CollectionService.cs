using System.Collections.Generic;
using System.Linq;

namespace DeiveEx.Utilities
{
    public static partial class UtilityServices
    {
        public static class CollectionService
        {
            public static bool IsNullOrEmpty<T>(IEnumerable<T> data) {
                return data == null || !data.Any();
            }
        }
    }
}
