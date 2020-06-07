using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroy<T>:MonoBehaviour where T:MonoBehaviour
{
    private static T _instance;

    private static object _lock = new object();

    public static T Instance
    {
        get {
            if (_isApplicationQuitting && Application.isPlaying)
            {
                DebugUtil.Warning("[SingletonDontDestroy] instance <" + typeof(T) + "> has destroied when application quitting, return null");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = "Singleton_" + typeof(T);

                    if (Application.isPlaying)
                    {
                        DontDestroyOnLoad(singleton);
                    }

                    DebugUtil.Log("[SingletonDontDestroy] instance <" + typeof(T) + "> has created");
                }

                return _instance;
            }
        }
    }

    private static bool _isApplicationQuitting = false;

    protected virtual void OnDestroy()
    {
        _isApplicationQuitting = true;
    } 
}

