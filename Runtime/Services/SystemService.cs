using System;
using UnityEngine;

namespace DeiveEx.Utilities
{
    public class SystemService
    {
        public bool HasCommandLineArgument(string key)
        {
            var args = Environment.GetCommandLineArgs();
            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == key)
                    return true;
            }

            return false;
        }
            
        public bool GetCommandLineArgument(string key, out string value)
        {
            var args = Environment.GetCommandLineArgs();
            value = "";
            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == key)
                {
                    if (args.Length > i)
                    {
                        value = args[i + 1];
                        return true;
                    }
                    else
                    {
                        Debug.LogError($"{key} has no value!");
                    }
                }
            }

            return false;
        }
    }        
}
