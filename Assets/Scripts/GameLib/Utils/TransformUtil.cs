using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtil
{
    public static Transform FindChild(Transform root,string name)
    {
        if (root==null)
        {
            return null;
        }

        Transform child = root.Find(name);
        if (child)
        {
            return child;
        }

        for(int i = 0; i < root.childCount; ++i)
        {
            Transform temp = root.GetChild(i);
            child = FindChild(temp, name);
            if (child)
            {
                return child;
            }
        }
        return child;
    }

    public static GameObject GetChildGameObject(GameObject root,string name)
    {
        Transform childTrans = FindChild(root.transform, name);
        if (childTrans)
        {
            return childTrans.gameObject;
        }
        return null;
    }
}
