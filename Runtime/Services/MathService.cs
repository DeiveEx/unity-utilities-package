using System;
using UnityEngine;

namespace DeiveEx.Utilities
{
    #region SubTypes

    public enum VectorAxis
    {
        X,
        Y, 
        Z
    }
    
    public enum SwizzleType
    {
        XYZ,
        XZY,
        YXZ,
        YZX,
        ZXY,
        ZYX
    }

    #endregion

    public static partial class UtilityServices
    {
        public class MathService
        {   
            public static Vector3 GetSwizzeledVector(Vector3 original, SwizzleType swizzleType)
            {
                switch (swizzleType)
                {
                    case SwizzleType.XYZ:
                        return original;
                    
                    case SwizzleType.XZY:
                        return new Vector3(original.x, original.z, original.y);
                    
                    case SwizzleType.YXZ:
                        return new Vector3(original.y, original.x, original.z);
                    
                    case SwizzleType.YZX:
                        return new Vector3(original.y, original.z, original.y);
                    
                    case SwizzleType.ZXY:
                        return new Vector3(original.z, original.x, original.y);
                    
                    case SwizzleType.ZYX:
                        return new Vector3(original.z, original.y, original.x);
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(swizzleType), swizzleType, null);
                }
            }
        }
    }
}
