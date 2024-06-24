using System;
using System.Collections.Generic;
using System.Linq;

namespace DeiveEx.Utilities
{
    public class BitwiseService
    {
        public int SetFlags(int value, int flagsToSet)
        {
            return value |= flagsToSet;
        }

        public int UnsetFlags(int value, int flagsToUnset)
        {
            return value &= ~flagsToUnset;
        }
            
        public bool HasFlags<T>(T value, T flags) where T : Enum
        {
            return value.HasFlag(flags);
        }

        public IEnumerable<T> GetFlagList<T>(T flags, params T[] excludeList) where T : Enum
        {
            List<T> flagList = new();
                
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                if(!excludeList.Contains(value) && flags.HasFlag(value))
                    flagList.Add(value);
            }

            return flagList;
        }
    }
}
