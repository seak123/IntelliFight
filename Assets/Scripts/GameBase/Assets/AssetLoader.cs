using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader:IPoolItem
{
    private System.WeakReference _obj;
    
    public Object asset
    {
        get { return _obj.Target as Object; }
    }

    public void BindAsset(Object asset)
    {
        _obj = new System.WeakReference(asset);
    }

    public bool IsAlive()
    {
        return asset != null;
    }

    public void UnLoad()
    {
        Resources.UnloadAsset(asset);
    }

    public void Reset()
    {
        this.UnLoad();
    }
}
