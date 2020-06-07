using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{ 

    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<T>();
                singleton.name = "Singleton_" + typeof(T);

                DebugUtil.Log("[Singleton] instance <" + typeof(T) + "> has created");
            }

            return _instance;
        }
    }

    
    protected virtual void OnDestroy()
    {
        _instance = null;
    }
}
