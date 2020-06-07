using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public struct SummonNodeVO
{ 
    public int uid;
    public UnitVO unitVO;
    public int positionX;
    public int positionZ;
}

public class SummonNode : PerformBaseNode
{
    private int unitId;
    private SummonNodeVO vo;
    private string unitPath;

    private MapCoordinates summonPos;
   public override void InjectData(LuaTable data)
    {
        vo = data.Get<SummonNodeVO>("nodeVO");
    }

    public override bool Perform(float delta)
    {
        MapManager.Instance.CreateUnit(vo.uid,vo.unitVO, new MapCoordinates(vo.positionX,vo.positionZ));
        return true;
    }


}
