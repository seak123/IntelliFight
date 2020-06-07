using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

enum NodeType
{
    Summon = 1,
}

[LuaCallCSharp]
public class Performer : Singleton<Performer>, IBattleManager
{
    List<PerformBaseNode> mainPerformQue;
    PerformBaseNode curMainNode; //正在播放的主节点
    List<PerformBaseNode> curSubNodes;
    // Start is called before the first frame update
    void Start()
    {
        mainPerformQue = new List<PerformBaseNode>(); //主干 播放队列
        curSubNodes = new List<PerformBaseNode>(); //正在播放的副节点
    }

    // Update is called once per frame
    void Update()
    {
        ExcuteSubNode();

        ExcuteMainNode();
    }

    public void Init()
    {
        LuaFuntionUtil.LuaEventRegister("PerfomerPushNode", PushNode);
    }

    public bool PushNode(LuaTable nodeVO)
    {

        mainPerformQue.Add(BuildMainNode(nodeVO));

        return true;
    }

    private PerformBaseNode BuildMainNode(LuaTable nodeVO)
    {
        PerformBaseNode node = null;
        NodeType type = (NodeType)nodeVO.Get<int>("nodeType");
        switch (type)
        {
            case NodeType.Summon:
                node = new SummonNode();
                node.InjectData(nodeVO);
                break;
        }
        node.childs = new List<PerformBaseNode>();
        int childCount = nodeVO.Get<int>("childCount");
        if (childCount > 0)
        {
            LuaTable childs = nodeVO.Get<LuaTable>("childs");
            for (int i = 0; i < childCount; ++i)
            {
                node.childs.Add(BuildMainNode(childs.Get<LuaTable>((i + 1).ToString())));
            }
        }
        return node;
    }

    //处理主干节点
    private void ExcuteMainNode()
    {

        if (curMainNode == null || curMainNode.Perform(Time.deltaTime))
        {
            if (mainPerformQue.Count == 0)
            {
                curMainNode = null;
                return;
            }

            //execute new main node
            curMainNode = mainPerformQue[0];
            mainPerformQue.RemoveAt(0);

            if (curMainNode.childs != null && curMainNode.childs.Count > 0)
            {
                for (int i = 0; i < curMainNode.childs.Count; ++i)
                {
                    curSubNodes.Add(curMainNode.childs[i]);
                }
            }
        }

    }

    //处理副节点
    private void ExcuteSubNode()
    {
        if (curSubNodes.Count > 0)
        {
            for (int i = curSubNodes.Count - 1; i >= 0; --i)
            {
                if (curSubNodes[i].delay > 0)
                {
                    curSubNodes[i].delay -= Time.deltaTime;
                }
                else
                {
                    if (curSubNodes[i].Perform(Time.deltaTime))
                    {
                        if (curSubNodes[i].childs != null && curSubNodes[i].childs.Count > 0)
                        {
                            for (int j = 0; j < curSubNodes[i].childs.Count; ++j)
                            {
                                curSubNodes.Add(curSubNodes[i].childs[j]);
                            }
                        }
                        curSubNodes.RemoveAt(i);
                    }
                }
            }
        }
    }
}
