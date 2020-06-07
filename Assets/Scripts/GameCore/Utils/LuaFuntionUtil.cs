using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

[CSharpCallLua]
public interface ILuaEventManager
{
    LuaTable RegisterCS(string eventName, Func<LuaTable, bool> tabel);

    void UnRegisterCS(string eventName, LuaTable handler);
    void Emit(string eventName, params object[] args);
}
public class LuaFuntionUtil
{
    static ILuaEventManager luaEventManger;

    private static void CheckInited()
    {
        if (luaEventManger == null)
        {
            var eventManger = LuaScriptManager.Instance.GetLuaEnv().Global.Get<ILuaEventManager>("EventManager");
            luaEventManger = eventManger;
        }
    }

    public static void LuaEventEmit(string eventName, params object[] args)
    {
        CheckInited();

        luaEventManger.Emit(eventName, args);
    }

    public static LuaTable LuaEventRegister(string eventName, Func<LuaTable, bool> action)
    {
        CheckInited();

        var handler = luaEventManger.RegisterCS(eventName, action);

        return handler;
    }

    public static void LuaEventUnRegister(string evetName,LuaTable handler)
    {
        CheckInited();

        luaEventManger.UnRegisterCS(evetName, handler);
    }
}
