using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class PerformBaseNode
{

    public List<PerformBaseNode> childs;

    public float delay = 0;
    public virtual void InjectData(LuaTable data) { }
    public virtual bool Perform(float delta) { return true; }

}
