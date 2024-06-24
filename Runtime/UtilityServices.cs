#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DeiveEx.Utilities
{
#if UNITY_EDITOR
    [InitializeOnLoad] //Initializes this class when Unity does a Domain Reload
#endif
    public static class UtilityServices
    {
        public static BitwiseService BitwiseService { get; }
        public static TilemapService TilemapService { get; }
        public static CollectionService CollectionService { get; }
        public static FileService FileService { get; }
        public static GameObjectService GameObjectService { get; }
        public static PhysicsService PhysicsService { get; }
        public static MathService MathService { get; }
        public static ReflectionService ReflectionService { get; }
        public static SystemService SystemService { get; }
        public static ThreadingService ThreadingService { get; }
        
        
        static UtilityServices()
        {
            BitwiseService = new();
            TilemapService = new();
            CollectionService = new();
            FileService = new();
            GameObjectService = new();
            PhysicsService = new();
            MathService = new();
            ReflectionService = new();
            SystemService = new();
            ThreadingService = new();
        }
    }
}
