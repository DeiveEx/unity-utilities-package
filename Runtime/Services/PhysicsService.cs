using System;
using UnityEngine;

namespace DeiveEx.Utilities
{
    public static partial class UtilityServices
    {
        public static class PhysicsService
        {
            public static bool CapsuleCast(CapsuleCollider collider, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
            {
                Transform capsuleTransform = collider.transform;
                Vector3 center = capsuleTransform.position + collider.center;
                float capsuleHalfHeight = collider.height / 2f;

                Vector3 capsuleDirection = collider.direction switch
                {
                    0 => capsuleTransform.right,
                    1 => capsuleTransform.up,
                    2 => capsuleTransform.forward,
                    _ => throw new ArgumentOutOfRangeException()
                };

                Vector3 p1 = center + capsuleDirection * capsuleHalfHeight;
                Vector3 p2 = center - capsuleDirection * capsuleHalfHeight;

                return Physics.CapsuleCast(p1, p2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            }
            
            public static bool CapsuleCast(CapsuleCollider collider, Vector3 direction, out RaycastHit hitInfo, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
            {
                return CapsuleCast(collider, collider.radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            }
            
            public static bool SphereCast(SphereCollider collider, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
            {
                Vector3 center = collider.center;
                return Physics.SphereCast(center, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            }
            
            public static bool SphereCast(SphereCollider collider, Vector3 direction, out RaycastHit hitInfo, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
            {
                return SphereCast(collider, collider.radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            }
            
            public static bool BoxCast(BoxCollider collider, Vector3 size, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
            {
                Vector3 center = collider.center;
                return Physics.BoxCast(center, size / 2f, direction, out hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction);
            }
            
            public static bool BoxCast(BoxCollider collider, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation, float maxDistance, LayerMask layerMask, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
            {
                return BoxCast(collider, collider.size / 2f, direction, out hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction);
            }

            public static bool IsLayerInLayerMask(int layer, LayerMask layerMask)
            {
                return (layerMask & (1 << layer)) != 0;
            }
        }
    }
}
