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
        public static BitwiseService BitwiseService { get; } = new();
        public static TilemapService TilemapService { get; } = new();
        public static CollectionService CollectionService { get; } = new();
        public static FileService FileService { get; } = new();
        public static GameObjectService GameObjectService { get; } = new();
        public static PhysicsService PhysicsService { get; } = new();
        public static MathService MathService { get; } = new();
        public static ReflectionService ReflectionService { get; } = new();
        public static SystemService SystemService { get; } = new();
        public static ThreadingService ThreadingService { get; } = new();
        public static UiService UiService { get; } = new();
    }
}
