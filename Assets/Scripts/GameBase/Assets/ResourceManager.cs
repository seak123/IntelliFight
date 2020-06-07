using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonDontDestroy<ResourceManager>, IManager
{
    private Pool<AssetLoader> loaderPool;
    private Dictionary<string, AssetLoader> loaderCache;

    ResourceManager()
    {
        loaderPool = new Pool<AssetLoader>();
        loaderCache = new Dictionary<string, AssetLoader>();
    }

    public void Init()
    {

    }
    public UnityEngine.Object LoadAssetSync(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        AssetLoader loader = null;
        loaderCache.TryGetValue(path, out loader);
        if (loader != null)
        {
            if (loader.IsAlive())
            {
                return loader.asset;
            }
            else
            {
                loaderPool.Store(loader);
                loaderCache.Remove(path);
            }

        }

        UnityEngine.Object asset = Resources.Load(path);
        loader = loaderPool.Fetch();
        loader.BindAsset(asset);
        loaderCache.Add(path, loader);
        return asset;
    }

    public void LoadAssetAsync(string path, Action<UnityEngine.Object> callback=null)
    {
        if (string.IsNullOrEmpty(path))
        {
            callback?.Invoke(null);
            return;
        }

        AssetLoader loader = null;
        loaderCache.TryGetValue(path, out loader);
        if (loader != null)
        {
            if (loader.IsAlive())
            {
                callback?.Invoke(loader.asset);
                return;
            }
            else
            {
                loaderPool.Store(loader);
                loaderCache.Remove(path);
            }

        }

        StartCoroutine(LoadAsyncCorotine(path,callback));

    }



    public void Clear()
    {
        
    }

    IEnumerator LoadAsyncCorotine(string path, Action<UnityEngine.Object> callback)
    {
        var request = Resources.LoadAsync(path);
        while (!request.isDone)
        {
            yield return null;
        }
        UnityEngine.Object asset = request.asset;
        AssetLoader loader = loaderPool.Fetch();
        loader.BindAsset(asset);
        loaderCache.Add(path, loader);
        callback(asset);
    }
}
