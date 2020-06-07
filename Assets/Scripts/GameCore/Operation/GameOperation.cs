using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public enum GestureType
{
    SingleTap,
    DounbleTap,
    Pinch,
    Swipe,
}

public struct GestureData
{
    public GestureType type;
    public Vector2 pos;
    public Vector2 offset;
    public float value;
}

//public enum OperationState
//{
//    Normal,
//    HoldCard,
//    SelectTarget,
//}

public class GameOperation : SingletonDontDestroy<GameOperation>, IManager
{
    // Start is called before the first frame update
    private int curTouchNum = 0; //有效的touch数量

    private readonly float MouseSwipeFactor = 1f; //win下模拟touch移动的调整系数
    private readonly float MousePinchFactor = 1f; //win下模拟pinch的调整系数

    private readonly float clickInterval = 0.3f;
    private readonly float doubleClickInterval = 0.3f;
    private readonly float longClickInterval = 0.3f;

    private readonly float clickOffsetBound = 20f; //点击前后 允许的位移边界

    private float holdElapesd = 0; //按住持续时间
    private float doubleTapElapsed = 0; //双击有效计时
    private bool doubleTapActive = false; //双击检测激活
    private bool longTapActive = false; //长按激活

    private Vector2 tapPos_0; //录入首次点击位置
    private Vector2 tapPos_1; //录入的第一个touch位置
    private int fingerId_1; //录入第一个touch的touchId
    private Vector2 tapPos_2; //录入的第二个touch位置

    private List<int> ignoreTouchs; //忽视掉的输入，存入fingerId
    private bool ignoreMouse = false; //忽视点击(仅在win操作)

    public Action<GestureData> onSingleDown;
    public Action<GestureData> onSingleTap;
    public Action<GestureData> onDoubleTap;
    public Action<GestureData> onLongTapDown;
    public Action<GestureData> onLongTap;
    public Action<GestureData> onLongTapUp;
    public Action<GestureData> onPinch;
    public Action<GestureData> onSwipe;

    void Start()
    {
        ignoreTouchs = new List<int>();
    }

    public void Init()
    {

    }

    public void Clear()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void LateUpdate()
    {

#if UNITY_IOS || UNITY_ANDROID
        if (curState == OperationState.Normal)
        {
            var touchs = new List<Touch>();
            //筛选有效touch
            for (int i = 0; i < Input.touches.Length; ++i)
            {
                if (Input.touches[i].phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(Input.touches[i].fingerId))
                {
                    ignoreTouchs.Add(Input.touches[i].fingerId);
                }
                if (IsTouchIgnored(Input.touches[i]) == -1)
                {
                    touchs.Add(Input.touches[i]);
                }
                if (Input.touches[i].phase == TouchPhase.Ended)
                {
                    int index = IsTouchIgnored(Input.touches[i]);
                    if (index > 0)
                    {
                        ignoreTouchs.RemoveAt(index);
                    }
                }
            }

            //double tap check
            if (doubleTapActive)
            {
                doubleTapElapsed += Time.deltaTime;
                if (doubleTapElapsed > doubleClickInterval)
                {
                    doubleTapActive = false;
                    doubleTapElapsed = 0;
                }
            }

            int lastTouchNum = curTouchNum;
            curTouchNum = touchs.Count;
            switch (curTouchNum)
            {
                case 0:
                    holdElapesd = 0;
                    break;
                case 1:
                    if (lastTouchNum == 0 && touchs[0].phase == TouchPhase.Began)
                    {
                        //初次点击
                        tapPos_1 = touchs[0].position;
                        holdElapesd = 0;
                    }
                    else if (lastTouchNum == 1)
                    {
                        if (touchs[0].phase == TouchPhase.Moved)
                        {
                            //Swipe
                            var data = new GestureData();
                            data.type = GestureType.Swipe;
                            data.offset = touchs[0].position - tapPos_1;
                            if (onSwipe != null)
                                onSwipe.Invoke(data);

                            tapPos_1 = touchs[0].position;
                        }
                        else if (touchs[0].phase == TouchPhase.Stationary)
                        {
                            holdElapesd += Time.deltaTime;
                        }
                        else if (touchs[0].phase == TouchPhase.Ended)
                        {
                            if (holdElapesd <= this.clickInterval)
                            {
                                var data = new GestureData();
                                //TODO data
                                if (onSingleTap != null)
                                    onSingleTap.Invoke(data);
                                holdElapesd = 0;
                                if (doubleTapActive)
                                {
                                    if (doubleTapElapsed <= doubleClickInterval)
                                    {
                                        var doubleData = new GestureData();
                                        //TODO data
                                        if (onDoubleTap != null)
                                            onDoubleTap.Invoke(doubleData);
                                        doubleTapActive = false;
                                    }
                                }
                                else
                                {
                                    doubleTapActive = true;
                                    doubleTapElapsed = 0;
                                }
                            }
                        }
                    }
                    break;
                case 2:
                default:
                    if (lastTouchNum < 2)
                    {
                        //首次记录位置
                        tapPos_1 = touchs[0].position;
                        tapPos_2 = touchs[1].position;
                    }
                    if (touchs[0].phase == TouchPhase.Moved || touchs[1].phase == TouchPhase.Moved)
                    {
                        //Pinch
                        var data = new GestureData();
                        data.type = GestureType.Pinch;
                        data.value = Vector2.Distance(touchs[0].position, touchs[1].position) - Vector2.Distance(tapPos_1, tapPos_2);
                        if (onPinch != null)
                            onPinch.Invoke(data);

                        tapPos_1 = touchs[0].position;
                        tapPos_2 = touchs[1].position;
                    }
                    break;
            }
        }
        else
        {
            if (Input.GetTouch(selectFingerId).phase == TouchPhase.Moved)
            {
                Vector3 pos = Input.GetTouch(selectFingerId).position;
                if (pos.y > cardExcuteBound)
                {
                    switch (curState)
                    {
                        case OperationState.HoldCard:
                            switch (selectCardType)
                            {
                                case CardExcuteType.SelectPos:
                                    SwitchOperationState(OperationState.SelectTarget);
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    SwitchOperationState(OperationState.HoldCard);
                }
            }

            if (Input.GetTouch(selectFingerId).phase == TouchPhase.Ended)
            {
                Vector3 pos = Input.GetTouch(selectFingerId).position;
                if (pos.y > cardExcuteBound)
                {
                    var data = new GestureData();
                    switch (curState)
                    {
                        case OperationState.HoldCard:

                            if (onReleaseCard != null)
                            {
                                onReleaseCard.Invoke(data);
                            }
                            break;
                        case OperationState.SelectTarget:
                            if (onReleaseCard != null)
                            {
                                onReleaseCard.Invoke(data);
                            }
                            break;
                    }
                }
                SwitchOperationState(OperationState.Normal);
            }
        }




#endif

#if UNITY_STANDALONE_WIN

        //double tap check
        if (doubleTapActive)
        {
            doubleTapElapsed += Time.deltaTime;
            if (doubleTapElapsed > doubleClickInterval)
            {
                doubleTapActive = false;
                doubleTapElapsed = 0;
            }
        }

        //check valid
        if (Input.GetMouseButtonUp(0))
        {
            ignoreMouse = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                ignoreMouse = true;
            }
            else
            {
                //初次点击
                tapPos_0 = Input.mousePosition;
                tapPos_1 = Input.mousePosition;
                holdElapesd = 0;

                var data = new GestureData();
                data.pos = tapPos_0;
                if (onSingleDown != null)
                {
                    onSingleDown.Invoke(data);
                }
            }

        }

        if (!ignoreMouse)
        {
            if (Input.GetMouseButton(0))
            {
                Vector2 curPos = Input.mousePosition;
                //longTap
                var dist = Vector2.Distance(curPos, tapPos_0);
                if (holdElapesd > this.longClickInterval)
                {
                    var longTapData = new GestureData();
                    longTapData.pos = tapPos_0;
                    if (dist <= clickOffsetBound && longTapActive == false)
                    {
                        longTapActive = true;
                        if (onLongTapDown != null)
                            onLongTapDown.Invoke(longTapData);
                    }
                    longTapData.pos = curPos;
                    if (onLongTap != null) onLongTap.Invoke(longTapData);
                }
                //Swipe
                var data = new GestureData();
                data.type = GestureType.Swipe;

                data.offset = (curPos - tapPos_1) * MouseSwipeFactor;

                if (onSwipe != null)
                    onSwipe.Invoke(data);

                tapPos_1 = Input.mousePosition;
                holdElapesd += Time.deltaTime;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (holdElapesd <= this.clickInterval && Vector2.Distance(tapPos_0, Input.mousePosition) <= clickOffsetBound)
                {
                    var data = new GestureData();
                    //TODO data
                    if (onSingleTap != null)
                        onSingleTap.Invoke(data);
                    holdElapesd = 0;
                    if (doubleTapActive)
                    {
                        if (doubleTapElapsed <= doubleClickInterval)
                        {
                            var doubleData = new GestureData();
                            //TODO data
                            if (onDoubleTap != null)
                                onDoubleTap.Invoke(doubleData);
                            doubleTapActive = false;
                        }
                    }
                    else
                    {
                        doubleTapActive = true;
                        doubleTapElapsed = 0;
                    }
                }
                if (longTapActive)
                {
                    longTapActive = false;
                    if (onLongTapUp != null)
                    {
                        GestureData data = new GestureData();
                        this.onLongTapUp.Invoke(data);
                    }
                }
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //Pinch
            var data = new GestureData();
            data.type = GestureType.Pinch;
            data.value = Input.GetAxis("Mouse ScrollWheel") * MouseSwipeFactor;
            if (onPinch != null)
                if (onPinch != null)
                    onPinch.Invoke(data);
        }

#endif
    }



    public bool MapPosSelector(out MapCoordinates pos)
    {
        Vector3 pointPos;
#if UNITY_IOS || UNITY_ANDROID
               if (Input.touches.Length > 0)
        {
            pointPos = Input.touches[0].position;
        }
#endif


#if UNITY_STANDALONE_WIN
        pointPos = Input.mousePosition;
#endif
        return MapPosSelector(pointPos, out pos);
    }
    public bool MapPosSelector(Vector2 screenPos, out MapCoordinates pos)
    {
        RaycastHit raycastHit;

        if (screenPos != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out raycastHit, 1000f, 1 << LayerMask.NameToLayer("Map")))
            {
                pos = MapCoordinates.FromPosition(raycastHit.point);
                return true;
            }
        }
        pos = MapCoordinates.None;
        return false;
    }
}
