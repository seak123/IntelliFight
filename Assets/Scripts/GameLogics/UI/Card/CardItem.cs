using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using XLua;

public enum CardExcuteType
{
    NoTarget = 0,
    SelectPos = 1,
    SelectUnit = 2,
}

public enum CardViewMode
{
    Normal,
    HoldCard,
    SelectTarget,
}

[LuaCallCSharp]
public class CardItem : MonoBehaviour
{
    public Button cardBtn;

    private readonly Vector2 displayPosition = new Vector2(100f, 400f);

    private bool holding = false;
    private Vector3 oriPos;
    private Transform oriParent;

    private int cardUid;
    private int cardId;
    private CardExcuteType cardExecuteType;
    private CardViewMode curViewMode;

    public int Uid
    {
        get
        {
            return cardUid;
        }
    }

    public CardExcuteType CardExecuteType
    {
        get
        {
            return cardExecuteType;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        curViewMode = CardViewMode.Normal;
        EventTriggerListener.Get(cardBtn.gameObject).onDown += OnTouchDown;
        EventTriggerListener.Get(cardBtn.gameObject).onUp += OnTouchUp;
    }

    public void InjectData(LuaTable cardVO)
    {
        cardUid = cardVO.Get<int>("uid");
        cardId = cardVO.Get<int>("id");
        cardExecuteType = (CardExcuteType)cardVO.Get<int>("cardExcuteType");
    }

    // Update is called once per frame
    void Update()
    {
        RefreshView();
    }

    void RefreshView()
    {
        switch (curViewMode)
        {
            case CardViewMode.Normal:
                break;
            case CardViewMode.HoldCard:
                if (holding)
                {
#if UNITY_IOS || UNITY_ANDROID
            if (Input.touches.Length > 0)
            {
                gameObject.transform.position = Input.GetTouch(GameOperation.Instance.SelectFingerId).position;
            }
#endif

#if UNITY_STANDALONE_WIN
                    gameObject.transform.position = Input.mousePosition;
#endif

                }
                break;
            case CardViewMode.SelectTarget:
                break;
        }
    }

    public void SwitchViewMode(CardViewMode newMode)
    {
        if (curViewMode == newMode) return;
        switch (newMode)
        {
            case CardViewMode.Normal:
                ResetCard();
                break;
            case CardViewMode.HoldCard:
                if(curViewMode == CardViewMode.Normal)
                {
                    
                    oriPos = gameObject.transform.position;
                    oriParent = gameObject.transform.parent;
                }
                iTween.Stop(gameObject);
                holding = true;
                WindowsUtil.SwitchLayer(gameObject, UILayer.NoticeLayer);
                break;
            case CardViewMode.SelectTarget:
                holding = false;
                iTween.MoveTo(gameObject, displayPosition, 0.5f);
                break;

        }
        curViewMode = newMode;
    }

    void OnTouchDown(PointerEventData data)
    {

        if (!IsPointerValid(data.pointerId))
        {
            return;
        }
        if (BattleManager.Instance.SelectCard(this))
        {
            SwitchViewMode(CardViewMode.HoldCard);
        }

    }

    void OnTouchUp(PointerEventData data)
    {
        if (!IsPointerValid(data.pointerId))
        {
            return;
        }
       
        BattleManager.Instance.ReleaseCard(this);
       
        SwitchViewMode(CardViewMode.Normal);
    }

    bool IsPointerValid(int pointId)
    {
#if UNITY_IOS || UNITY_ANDROID
            if (Input.touches.Length == 0 || Input.touches[0].fingerId != pointId)
        {
            return false;
        }
#endif

#if UNITY_STANDALONE_WIN
        if (pointId != -1)
        {
            return false;
        }
#endif
        return true;
    }

    void ResetCard()
    {
        holding = false;
        iTween.Stop(gameObject);
        gameObject.SetActive(true);
        gameObject.transform.SetParent(oriParent);
        gameObject.transform.position = oriPos;
    }

}
