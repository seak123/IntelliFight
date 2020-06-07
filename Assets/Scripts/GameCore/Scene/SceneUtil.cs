using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[XLua.LuaCallCSharp]
public static class SceneUtil
{
    public static void SwitchScene(string newSceneName,System.Action callback)
    {
        GameSceneManager.Instance.LoadScene(newSceneName, callback);
    }
}
