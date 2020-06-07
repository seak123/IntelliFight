using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWindow : MonoBehaviour
{
    private Slider loadingSlider;
    private Text processValue;

    private float virtualProcess;
    private float trueProcess;

    private const float MAX_ADD_FRAME = 0.05f; //最快每帧增速
    private const float MIN_ADD_FRAME = 0.002f; //最慢每帧增速
    
    void Awake()
    {
        loadingSlider = gameObject.GetComponentInChildren<Slider>();
        processValue = gameObject.GetComponentInChildren<Text>();

        virtualProcess = 0f;
        trueProcess = 0f;
    }

    public void SetProcess(float process)
    {
        trueProcess = process;
        
    }

    private void Update()
    {
        float change = trueProcess - virtualProcess;
        change = Mathf.Min(MAX_ADD_FRAME, change);
        change = Mathf.Max(MIN_ADD_FRAME, change);
        virtualProcess += change;
        RefreshView();

        if (virtualProcess >= 1f)
        {
            WindowsUtil.RemoveWindow(this.gameObject);
        }
    }

    private void RefreshView()
    {
        float viewValue = Mathf.Clamp01(virtualProcess);
        processValue.text = "加载中..." + Mathf.RoundToInt(viewValue * 100) + "%";

        loadingSlider.value = viewValue;
    }

}
