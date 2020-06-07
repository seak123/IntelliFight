using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot :MonoBehaviour
{
    public GameObject[] dontDestroyObjs;
    private ResourceRequest _request;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        for(int i = 0; i < dontDestroyObjs.Length; ++i)
        {
            DontDestroyOnLoad(dontDestroyObjs[i]);
        }
    }

    void Start()
    {
        InitManagers();
    }

    void Update()
    {
      
    }

    private void InitManagers()
    {
        EventManager.Instance.Init();
        LuaScriptManager.Instance.Init();
        ResourceManager.Instance.Init();
        UIManager.Instance.Init();
        GameSceneManager.Instance.Init();
        GameOperation.Instance.Init();
    }
}
