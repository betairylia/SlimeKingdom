using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Toolbox
{
    public static string ToRawString(Vector3 vec)
    {
        return vec.x + " " + vec.y + " " + vec.z;
    }

    public static Vector3 ToVec3(string x, string y, string z)
    {
        return new Vector3(float.Parse(x), float.Parse(y), float.Parse(z));
    }

	public static bool QuaternionEquals(Quaternion q1, Quaternion q2)
	{
		return ((q1 * Vector3.forward - q2 * Vector3.forward).magnitude < 0.001f ? true : false);
	}

    public static bool VectorEquals(Vector2 a, Vector2 b)
    {
        return (Vector2.Distance(a, b) <= 1E-03 ? true : false);
    }

    public static bool VectorEquals(Vector3 a, Vector3 b)
    {
        return (Vector3.Distance(a, b) <= 1E-03 ? true : false);
    }

    public static bool VectorEquals(Vector4 a, Vector4 b)
    {
        return (Vector4.Distance(a, b) <= 1E-03 ? true : false);
    }
}
