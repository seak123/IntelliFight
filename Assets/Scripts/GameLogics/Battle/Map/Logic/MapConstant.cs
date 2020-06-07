using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapConstant 
{
    public const float sideLength = 10f;
    public const float lineLength = 0.5f;

    public static Vector3[] corners =
    {
        new Vector3(0f+lineLength,0f,sideLength-lineLength),
        new Vector3(sideLength-lineLength,0f,sideLength-lineLength),
        new Vector3(sideLength-lineLength,0f,0f+lineLength),
        new Vector3(0f+lineLength,0f,0f+lineLength),
    };
}
