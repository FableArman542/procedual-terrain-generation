using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3ExtensionMethods
{
    /// <summary>
    /// returns magnitude excluding y component as float
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float GetFlatMagnitude(this Vector3 v)
    {
        Vector3 flatV = new Vector3(v.x, 0, v.z);
        return flatV.magnitude;
    }
    /// <summary>
    /// returns vector3 with y component of 0
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector3 GetFlatVector3 (this Vector3 v)
    {
        Vector3 flat = new Vector3(v.x, 0, v.z);
        return flat;
    }
}
