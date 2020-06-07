using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolItem
{
    void Reset();
}

public class Pool<T> where T : IPoolItem
{
    private Stack<T> items = new Stack<T>();
    private int capacity;
    public Pool(int num=10)
    {
        capacity = num;
    }

    public T Fetch()
    {
        if (items.Count > 0)
        {
            return items.Pop();
        }
        return Activator.CreateInstance<T>();
    }

    public void Store( T item)
    {
        if (items.Count >= capacity)
        {
            return;
        }
        item.Reset();
        items.Push(item);
    }
}
