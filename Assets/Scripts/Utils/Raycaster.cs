using UnityEngine;

namespace SnowHorse.Utils
{
    public static class Raycaster
    {
        public static GameObject Cast(Vector3 origin, Vector3 direction, float maxDistance, LayerMask layerMask, out Vector3 hitPoint, bool debugRay = false, Color debugColor = new Color())
        {
            Ray ray = new Ray();
            ray.origin = origin;
            ray.direction = direction;
            hitPoint = GetHitPoint(ray, maxDistance, layerMask, out GameObject objectHit, debugRay, debugColor);
            return objectHit;
        }

        public static GameObject Cast(Ray ray, Vector3 direction, float maxDistance, LayerMask layerMask, out Vector3 hitPoint, bool debugRay = false, Color debugColor = new Color())
        {
            ray.direction = direction;
            hitPoint = GetHitPoint(ray, maxDistance, layerMask, out GameObject objectHit, debugRay, debugColor);
            return objectHit;
        }

        private static Vector3 GetHitPoint(Ray ray, float maxDistance, LayerMask layerMask, out GameObject objectHit, bool debugRay = false, Color debugColor = new Color())
        {
            Vector3 hitPoint;
            RaycastHit hit;

            objectHit = Physics.Raycast(ray, out hit, maxDistance, layerMask) ? hit.collider.gameObject : null;
            hitPoint = objectHit ? hit.point : Vector3.zero;

            if (debugRay) Debug.DrawRay(ray.origin, ray.direction * maxDistance, debugColor);
            return hitPoint;
        }

        public static GameObject Cast2D(Vector3 origin, Vector3 direction, float maxDistance, LayerMask layerMask, out Vector3 hitPoint, bool debugRay = false, Color debugColor = new Color())
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);
            GameObject objectHit;

            objectHit = hit.collider ? hit.collider.gameObject : null;
            hitPoint = objectHit ? hit.point : Vector3.zero;

            if (debugRay) Debug.DrawRay(origin, direction * maxDistance, debugColor);
            return objectHit;
        }
    }
}