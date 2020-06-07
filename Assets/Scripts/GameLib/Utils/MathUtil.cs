using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtil
{
    static void FloatRound(ref float value,int decimalPlace)
    {
        float ratio = Mathf.Pow(10, decimalPlace);
        value = Mathf.Round(value * ratio) / ratio;
    }
}
