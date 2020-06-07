using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using XLua;

[LuaCallCSharp]
public class GameSceneManager : SingletonDontDestroy<GameSceneManager>,IManager
{
    private int currSceneId;

    private Action onSceneLoaded;
    private Action onSceneLoadedOnce;
    public int CurrSceneId
    {
        get{
            return currSceneId;
        }
    }

    public void Init()
    {
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) => {
            onSceneLoaded.Invoke();
            onSceneLoadedOnce.Invoke();
            //clear once delegate
            var delegates = onSceneLoadedOnce.GetInvocationList();
            for(int i = 0; i < delegates.Length; ++i)
            {
                onSceneLoadedOnce -= delegates[i] as Action;
            }
        };
    }

    public void AddSceneLoadedListener(Action callback)
    {
        onSceneLoaded += callback;
    }

    public void RemoveSceneLoadedListener(Action callback)
    {
        onSceneLoaded -= callback;
    }

    public void AddSceneLoadedOnceListener(Action callback)
    {
        onSceneLoadedOnce += callback;
    }

    public void LoadScene(string sceneName,System.Action callback)
    {
        GameObject window = WindowsUtil.AddWindow("Loading/UI_Loading");
        UIManager.Instance.SwitchLayer(window, UILayer.NoticeLayer);
        LoadWindow script = window.GetComponent<LoadWindow>();
        StartCoroutine(LoadSceneCorotine(sceneName,script.SetProcess,callback));
    }

    IEnumerator LoadSceneCorotine(string sceneName, System.Action<float> ProcessCall,System.Action FinishedCall)
    {
        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            if (ProcessCall != null)
            {
                ProcessCall(operation.progress);
            }

            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }

        FinishedCall?.Invoke();
    }

    public void Clear()
    {

    }
}
