using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[CSharpCallLua]
public struct UnitVO
{
    public int id;
    public int speed;
    public int hp;
    public int attack;
    public int attackRange;
    public int mobility;
    public string prefabPath;
}
public class Unit
{
    public int uid;
    public UnitVO vo;
    public UnitAvatar avatar;
    public MapCoordinates pos;
}
