using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensionMethods
{
    /// <summary>
    /// Use in Update.
    /// The current offset follow is optimized for a quick attachment and may lead to drifting
    /// </summary>
    /// <param name="t"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public static Vector3 FollowWithCurrentOffset(this Transform t, Transform target)
    {
        Vector3 v = target.position - t.position;
        t.position = target.position - v;
        return t.position;
    }
    /// <summary>
    /// Use in Update.
    /// The defined offset is optimized for a cached offset value and prevents drifting
    /// </summary>
    /// <param name="t"></param>
    /// <param name="target"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public static Vector3 FollowWithDefinedOffset(this Transform t, Transform target, Vector3 offset)
    {
        t.position = target.position - offset;
        return t.position;
    }
}
