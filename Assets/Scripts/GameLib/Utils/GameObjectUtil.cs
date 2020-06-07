using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[XLua.LuaCallCSharp]
public static class GameObjectUtil
{
   public static GameObject FindGameObjectStrictly(GameObject obj,string path)
    {
        var childTrans =  obj.transform.Find(path);
        return childTrans != null ? childTrans.gameObject : null;
    }

    public static GameObject FindGameObject(GameObject root,string name)
    {
        return TransformUtil.GetChildGameObject(root, name);
    }
}
