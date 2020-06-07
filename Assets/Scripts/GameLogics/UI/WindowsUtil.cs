using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[XLua.LuaCallCSharp]
public static class WindowsUtil
{
    public static GameObject AddWindow(string path)
    {
       GameObject obj = UIManager.Instance.CreateUIPrefab(path);
        return obj;
    }

    public static void AddWindowAsync(string path, Action<UnityEngine.Object> callback)
    {
        UIManager.Instance.CreateUIPrefabAsync(path, callback);
    }

    public static void RemoveWindow(GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    public static void SwitchLayer(GameObject obj,UILayer layer)
    {
        UIManager.Instance.SwitchLayer(obj, layer);
    }

}
