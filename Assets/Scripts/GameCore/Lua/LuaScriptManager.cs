using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;

public class LuaScriptManager : SingletonDontDestroy<LuaScriptManager>,IManager
{
    private static LuaEnv _luaEnv;
    private LuaFunction _mainProcess;
    private LuaFunction _mainUpdate;
    LuaScriptManager()
    {
        _luaEnv = new LuaEnv();

        //load lua script
        LuaEnv.CustomLoader loader = CustomLoaderMethod;
        _luaEnv.AddLoader(loader);
        
    }

    public void Init()
    {
        var luaEnv = LuaScriptManager.Instance.GetLuaEnv();
        luaEnv.DoString("require 'GameCore.Main.MainProcedure'");
        _mainProcess = luaEnv.Global.Get<XLua.LuaFunction>("Main");
        _mainUpdate = luaEnv.Global.Get<XLua.LuaFunction>("MainUpdate");
        LuaBehaviourManager.Instance.Init();
    }

    void Start()
    {
        if (_mainProcess != null)
        {
            _mainProcess.Call();
        }
    }

    void Update()
    {
        if (_mainUpdate!=null) {
            _mainUpdate.Call(Time.deltaTime);
        }
    }

    public LuaEnv GetLuaEnv()
    {
        return _luaEnv;
    }

    public void Clear()
    {
        LuaBehaviourManager.Instance.Clear();
    }

    private byte[] CustomLoaderMethod(ref string filePath)
    {
        DebugUtil.Log("load script: " + filePath);
        filePath = Application.streamingAssetsPath + "/LuaScripts/" + filePath.Replace('.', '/') + ".lua";

        if (File.Exists(filePath))
        {
            return File.ReadAllBytes(filePath);
        }
        else
        {
            return null;
        }
    }


}
