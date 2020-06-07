using System;

using UnityEngine.Events;
using XLua;

[LuaCallCSharp]
public static class EventDelegate
{ 
    public static UnityAction CreateVoidUnityAction(LuaFunction function)
    {
        return new UnityAction(() => {
            function.Call();
        });
    }
}
