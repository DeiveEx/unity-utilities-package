using System;
using UnityEngine;

namespace DeiveEx.Utilities
{ 
    [Serializable]
    public struct MinMax
    {
        public float min;
        public float max;

        public MinMax(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
        
        //Get random value between the min and max values
        public float GetRandom() => UnityEngine.Random.Range(min, max);

        public static implicit operator Vector2(MinMax value) => new Vector2(value.min, value.max);
        public static implicit operator MinMaxInt(MinMax value) => new MinMaxInt((int)value.min, (int)value.max);
    }

    [Serializable]
    public struct MinMaxInt
    {
        public int min;
        public int max;

        public MinMaxInt(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
        
        public float GetRandom() => UnityEngine.Random.Range(min, max);

        public static implicit operator Vector2Int(MinMaxInt value) => new Vector2Int(value.min, value.max);
        public static implicit operator MinMax(MinMaxInt value) => new MinMax(value.min, value.max);
    }
}