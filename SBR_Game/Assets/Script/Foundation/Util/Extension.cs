using System;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extension
{
    public static Vector2 ConvertVec2(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.z);
    }
}
