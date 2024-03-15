using UnityEngine;

public class Raycaster : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private GameObject objectHit;

    public GameObject Cast(Vector3 origin, Vector3 direction, float maxDistance, LayerMask layerMask, out Vector3 hitPoint, bool debugRay = false, Color debugColor = new Color())
    {
        ray.origin = origin;
        ray.direction = direction;
        hitPoint = GetHitPoint(ray, maxDistance, layerMask, debugRay, debugColor);
        return objectHit;
    }

    public GameObject Cast(Ray ray, Vector3 direction, float maxDistance, LayerMask layerMask, out Vector3 hitPoint, bool debugRay = false, Color debugColor = new Color())
    {
        ray.direction = direction;
        hitPoint = GetHitPoint(ray, maxDistance, layerMask, debugRay, debugColor);
        return objectHit;
    }

    private Vector3 GetHitPoint(Ray ray, float maxDistance, LayerMask layerMask, bool debugRay = false, Color debugColor = new Color())
    {
        Vector3 hitPoint;

        objectHit = Physics.Raycast(ray, out hit, maxDistance, layerMask) ? hit.collider.gameObject : null;
        hitPoint = objectHit ? hit.point : Vector3.zero;

        if (debugRay) Debug.DrawRay(ray.origin, ray.direction * maxDistance, debugColor);
        return hitPoint;
    }

    public GameObject Cast2D(Vector3 origin, Vector3 direction, float maxDistance, LayerMask layerMask, out Vector3 hitPoint, bool debugRay = false, Color debugColor = new Color())
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);

        objectHit = hit.collider ? hit.collider.gameObject : null;
        hitPoint = objectHit ? hit.point : Vector3.zero;

        if (debugRay) Debug.DrawRay(origin, direction * maxDistance, debugColor);
        return objectHit;
    }
}