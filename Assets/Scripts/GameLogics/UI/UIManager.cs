using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[XLua.LuaCallCSharp]
public enum UILayer
{
    MainLayer_0,
    NoticeLayer,
}

public class UIManager:SingletonDontDestroy<UIManager>,IManager
{
    const string UI_RELATIVE_PATH = "UI/Prefabs/";

    private GameObject _managerObj;
    private GameObject _mainLayer_0_Obj;
    private GameObject _noticeLayerObj;
    
    public void Init()
    {

    }

    public void Awake()
    {
        _managerObj = GameObject.Find("UIManager");
        _mainLayer_0_Obj = _managerObj.transform.Find("MainLayer0").gameObject;
        _noticeLayerObj = _managerObj.transform.Find("NoticeLayer").gameObject;
    }
    public GameObject CreateUIPrefab(string path)
    {
        GameObject prefab = ResourceManager.Instance.LoadAssetSync(PathTransformer(path)) as GameObject;
        GameObject obj;
   
        obj = Instantiate(prefab,_mainLayer_0_Obj.transform,false);
               
        return obj;
     
    }

    public void CreateUIPrefabAsync(string path, System.Action<UnityEngine.Object> callback)
    {
        ResourceManager.Instance.LoadAssetAsync(PathTransformer(path),(asset)=>
        {
            GameObject obj = Instantiate(asset as GameObject, _mainLayer_0_Obj.transform, false);
            callback(obj);
        });
    }

    public void SwitchLayer(GameObject obj, UILayer layer)
    {
        switch (layer)
        {
            case UILayer.MainLayer_0:
                obj.transform.SetParent(_mainLayer_0_Obj.transform);
                break;
            case UILayer.NoticeLayer:
                obj.transform.SetParent(_noticeLayerObj.transform);
                break;
        }
    }

    public void Clear()
    {

    }

    private string PathTransformer(string path)
    {
        return UI_RELATIVE_PATH + path;
    }

}
