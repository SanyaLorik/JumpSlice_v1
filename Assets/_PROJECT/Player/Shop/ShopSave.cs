using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopSave
{
    public int IdSelect = 0;

    public List<ShopItemSave> Skins = new()
    {
        new ShopItemSave()
        {
            Id = 0,
            IsBought = true
        }
    };
}

[Serializable]
public class ShopItemSave 
{
    public int Id;
    public bool IsBought;

    [Header("Ad")]
    public bool IsAd;
    public int MaxCountAd;
    public int CurrentCountAd;
}