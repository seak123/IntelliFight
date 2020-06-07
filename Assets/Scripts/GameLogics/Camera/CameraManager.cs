using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

enum CameraState
{
    Normal, //战斗中的普通模式
    Focus, //战斗中聚焦棋盘模式
}

[LuaCallCSharp]
public class CameraManager : Singleton<CameraManager>, IBattleManager
{
    private const float battlePinchFactor = 8f;
    private const float battleSwipeFactor = 0.1f;

    private const float viewMax = 70f;
    private const float viewMin = 40f;

    private CameraState state;
    private Vector3 oriRotation; //记录相机角度
    private Vector3 oriPos; //记录相机位置
    private float orifieldOfView; //记录相机视野大小

    private readonly Vector3 focusRotation = new Vector3(90, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        GameSceneManager.Instance.AddSceneLoadedListener(() => { Init(); });
    }

    public void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //初始化场景镜头
    public void InitSceneCamera(int sceneId)
    {
        state = CameraState.Normal;
        //临时参数
        Camera.main.transform.position = new Vector3(32, 110, 16);
        Camera.main.transform.rotation = Quaternion.Euler(80, 0, 0);
        //临时事件绑定
        GameOperation.Instance.onPinch += OnPinchInBattle;
        GameOperation.Instance.onSwipe += OnSwipeInBattle;
        BattleManager.Instance.onOperationModeChange += OnOperationStateChange;
    }

    //在战场中捏镜头
    private void OnPinchInBattle(GestureData data)
    {
        if (BattleManager.Instance.CurMode != BattleOperationMode.Normal) return;
        Camera.main.fieldOfView -= data.value * battlePinchFactor;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, viewMin, viewMax);
    }

    //在战场中拖镜头
    private void OnSwipeInBattle(GestureData data)
    {
        if (BattleManager.Instance.CurMode != BattleOperationMode.Normal) return;
        Camera.main.transform.position -= new Vector3(data.offset.x * battleSwipeFactor, 0, data.offset.y * battleSwipeFactor);
    }

    private void OnOperationStateChange(BattleOperationMode current)
    {
        switch (current)
        {
            case BattleOperationMode.Normal:
            case BattleOperationMode.HoldCard:
                SwitchCameraState(CameraState.Normal);
                break;
            case BattleOperationMode.SelectTarget:
            case BattleOperationMode.SelectPath:
                SwitchCameraState(CameraState.Focus);
                break;
        }
    }

    private void SwitchCameraState(CameraState newState)
    {
        if (state == newState) return;
        switch (newState)
        {
            case CameraState.Normal:
                state = CameraState.Normal;
                LerpCameraPosition(0.5f, oriPos, oriRotation, orifieldOfView);
                break;
            case CameraState.Focus:
                oriPos = Camera.main.transform.position;
                oriRotation = Camera.main.transform.rotation.eulerAngles;
                orifieldOfView = Camera.main.fieldOfView;

                state = CameraState.Focus;

                var center = MapManager.Instance.MapCenter;
                LerpCameraPosition(0.5f, new Vector3(center.x, oriPos.y, center.y), focusRotation, 70);
                break;
        }
    }

    private void LerpCameraPosition(float duration, Vector3 position, Vector3 rotation, float fieldOfView)
    {
        iTween.MoveTo(Camera.main.gameObject, position, duration);
        iTween.RotateTo(Camera.main.gameObject, rotation, duration);
        iTween.ValueTo(gameObject, iTween.Hash("from", Camera.main.fieldOfView, "to", fieldOfView,
             "easetype", iTween.EaseType.easeInOutBack, "loopType", iTween.LoopType.none, "onupdate", "FieldOfViewUpdate", "time", 0.5f));

    }

    private void FieldOfViewUpdate(float value)
    {
        Camera.main.fieldOfView = value;
    }
}
