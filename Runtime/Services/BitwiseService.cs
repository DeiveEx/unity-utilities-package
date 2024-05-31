using System;
using System.Collections.Generic;
using System.Linq;

namespace DeiveEx.Utilities
{
    public static partial class UtilityServices
    {
        public static class BitwiseService
        {
            public static int SetFlags(int value, int flagsToSet)
            {
                return value |= flagsToSet;
            }

            public static int UnsetFlags(int value, int flagsToUnset)
            {
                return value &= ~flagsToUnset;
            }
            
            public static bool HasFlags<T>(T value, T flags) where T : Enum
            {
                return value.HasFlag(flags);
            }

            public static IEnumerable<T> GetFlagList<T>(T flags, params T[] excludeList) where T : Enum
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
}
