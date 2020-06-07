using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public interface ILuaBehaviourManager
{
    LuaTable CreateBehaviour(string typeName, GameObject go);

    void CleanBehaviourCache(string typeName);

    void Update();
}
public class LuaBehaviourManager : SingletonDontDestroy<LuaBehaviourManager>, IManager
{
    private ILuaBehaviourManager _manager;
    public void Init()
    {

        _manager = LuaScriptManager.Instance.GetLuaEnv().Global.Get<ILuaBehaviourManager>("BehaviourManager");
    }

    public LuaTable CreateBehaviour(string typeName, GameObject obj)
    {
        var table = _manager.CreateBehaviour(typeName, obj);
        return table;
    }

    public void CleanBehaviourCache(string typeName)
    {
        _manager.CleanBehaviourCache(typeName);
    }

    public void Clear()
    {

    }

    void Update()
    {
        if (_manager != null)
            _manager.Update();
    }
}
