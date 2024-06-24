using System.Collections.Generic;
using System.Linq;

namespace DeiveEx.Utilities
{
    public class CollectionService
    {
        public bool IsNullOrEmpty<T>(IEnumerable<T> data) {
            return data == null || !data.Any();
        }
    }
}
