using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapCoordinates
{
    [SerializeField]
    private int x, z;
    public int X
    {
        get
        {
            return x;
        }
    }
    public int Z {
        get {
            return z;
        }
    }

    public MapCoordinates(int x,int z)
    {
        this.x = x;
        this.z = z;
    }

    public static MapCoordinates None
    {
        get
        {
            return new MapCoordinates(-1, -1);
        }
    }


    public static MapCoordinates FromPosition(Vector3 position)
    {
        float x = (position.x-MapConstant.sideLength*0.5f) / (MapConstant.sideLength);
        float z = (position.z - MapConstant.sideLength * 0.5f) / (MapConstant.sideLength);

        int iX = Mathf.RoundToInt(x);
        int iZ = Mathf.RoundToInt(z);

        return new MapCoordinates(iX, iZ);
    }

    public static int Distance(MapCoordinates start,MapCoordinates end)
    {
        return Mathf.Abs(start.X - end.X) + Mathf.Abs(start.Z - end.Z);
    }

    public Vector3 WorldPosition
    {
        get{
            float posX = x*MapConstant.sideLength + MapConstant.sideLength * 0.5f;
            float posZ = z * MapConstant.sideLength + MapConstant.sideLength * 0.5f;

            return new Vector3(posX, 0, posZ);
        } 

    }

    public bool Equal(MapCoordinates another)
    {
        return another.X == x && another.Z == z;
    }

    public override string ToString()
    {
        return "(" + X.ToString()  + "," + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines()
    {
        return X.ToString()  + "\n" + Z.ToString();
    }
}

public class MapCell : MonoBehaviour
{
    public MapCoordinates coordinates;
    public Color color;
}
