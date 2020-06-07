using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public static class LuaCallCSharpUtil
{
    public static Vector3 CreateVector3(float x,float y,float z)
    {
        return new Vector3(x, y, z);
    }
}
