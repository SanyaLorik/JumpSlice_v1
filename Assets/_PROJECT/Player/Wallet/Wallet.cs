using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _count;

    public Action<int> OnChanged;

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
        Change(-_count);
    }

    private void Change(int count)
    {
        _count += count;
        OnChanged?.Invoke(_count);
    }
}
