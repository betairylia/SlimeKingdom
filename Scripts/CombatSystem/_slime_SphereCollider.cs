using UnityEngine;
using System.Collections;

public class _slime_SphereCollider
{
    public _slime_SphereCollider(Vector3 c, float r)
    {
        centerPos = c;
        radius = r;
    }

    public void SetProps(Vector3 c, float r)
    {
        centerPos = c;
        radius = r;
    }

    public bool Intersects(_slime_SphereCollider coll)
    {
        return ((coll.centerPos - centerPos).magnitude <= coll.radius + radius);
    }

    public Vector3 centerPos;
    public float radius;
}
