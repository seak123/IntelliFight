using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

[LuaCallCSharp]
public class EventManager : ObjectSingleton<EventManager>,IManager
{
    //Dictionary<EventEnum, event>> eventHandlers;
    //private Action<object[]> events;
   public void Init()
    {
        //eventHandlers = new Dictionary<EventEnum, Action<object[]>>();
    }

    /*public void Register(EventEnum eventName, Action<object[]> eventHandler)
    {
        if (!eventHandlers.ContainsKey(eventName))
        {
            eventHandlers.Add(eventName, Action<object[]>(object[]));
        }
    }

    public void Emit(EventEnum eventName,params object[] args)
    {

    }*/

    public void Clear()
    {

    }
}
