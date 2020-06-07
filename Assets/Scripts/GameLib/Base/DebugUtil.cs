using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtil
{
    public static void Log(params object[] messages)
    {
        string str = "";
        for (int i = 0; i < messages.Length; ++i)
        {
            str += messages[i].ToString();
        }
        UnityEngine.Debug.Log(str);
    }

    public static void Warning(object message,UnityEngine.Object _object = null)
    {
        UnityEngine.Debug.LogWarning(message, _object);
    }

    public static void Error(params object[] messages)
    {
        string str = "";
        for(int i = 0; i < messages.Length; ++i)
        {
            str += messages[i].ToString();
        }
        UnityEngine.Debug.LogError(str);
    }
}
