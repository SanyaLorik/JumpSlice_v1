using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [field: SerializeField] public int Count { get; private set; }

    public Action<int> OnChanged;

    public void Set(int count)
    {
        Clear();
        Change(count);
    }

    public void Add(int count)
    {
        Change(count);
    }

    public void Remove(int count)
    {
        Change(-count);
    }

    public void Clear()
    {
        Change(-Count);
    }

    private void Change(int count)
    {
        Count += count;
        OnChanged?.Invoke(Count);
    }
}