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

    public class MathService
    {   
        public Vector3 GetSwizzeledVector(Vector3 original, SwizzleType swizzleType)
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
        
        public Vector3 QuadraticBezier (Vector3 p0, Vector3 p2, Vector3 middlePoint, float t) {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * middlePoint + t * t * p2;
        }
    }
}
