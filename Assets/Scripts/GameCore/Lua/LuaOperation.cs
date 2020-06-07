using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using XLua;
    
[CSharpCallLua]
public delegate void VoidEvent();

[LuaCallCSharp]
public class LuaOperation : MonoBehaviour 
{
    public string classPath = "";

    public event VoidEvent AwakeEvent;
    public event VoidEvent StartEvent;
    public event VoidEvent EnableEvent;
    public event VoidEvent DisableEvent;
    public event VoidEvent DestroyEvent;

    private LuaTable _luaBehaviour;

    public LuaTable luaBehaviour
    {
        get { return _luaBehaviour; }
    }

    public void ResetClassPath(string path)
    {
        classPath = path;
        if (!String.IsNullOrEmpty(classPath))
        {
            _luaBehaviour = LuaBehaviourManager.Instance.CreateBehaviour(classPath, this.gameObject);
        }

        if (AwakeEvent != null)
        {
            AwakeEvent();
        }
        if (EnableEvent != null)
        {
            EnableEvent();
        }
        if (StartEvent != null)
        {
            StartEvent();
        }

    }
    
    void Awake()
    {
        if (!String.IsNullOrEmpty(classPath))
        {
            _luaBehaviour = LuaBehaviourManager.Instance.CreateBehaviour(classPath, this.gameObject);
        }
        
        if (AwakeEvent != null) 
        {
            AwakeEvent();
        }
    }

    void Start() 
    {
        if (StartEvent != null) 
        {
            StartEvent();
        }
    }

    void OnEnable()
    {
        if (EnableEvent != null) 
        {
            EnableEvent();
        }
    }

    void OnDisable()
    {
        if (DisableEvent != null) 
        {
            DisableEvent();
        }
    }

    void OnDestroy()
    {
        if (DestroyEvent != null) 
        {
            DestroyEvent();
        }

        this.Clear();
    }

    private void Clear()
    {
        this.AwakeEvent = null;
        this.StartEvent = null;
        this.EnableEvent = null;
        this.DisableEvent = null;
        this.DisableEvent = null;

        this.StopAllCoroutines();
        this.coBodys = null;

#if UNITY_EDITOR
       // LuaBehaviourManager.Instance.CleanBehaviourCache(classPath);
#endif
    }

    private Dictionary<object, IEnumerator> coBodys = new Dictionary<object, IEnumerator>();

    public void YieldAndCallback(object to_yield, Action callback)
    {
        var body = CoBody(to_yield, callback);

        coBodys[to_yield] = body;

        StartCoroutine(body);
    }

    public void StopYield(object to_yield)
    {
        if (!coBodys.ContainsKey(to_yield))
        {
            Debug.LogError("No Target to stop");
        }

        this.StopCoroutine(coBodys[to_yield]);
    }

    private IEnumerator CoBody(object to_yield, Action callback)
    {
        if (to_yield is IEnumerator)
        {
            yield return StartCoroutine((IEnumerator)to_yield);
        }
        else
        {
            yield return to_yield;
        }

        callback();
    }
}