using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopSave
{
    public int IdSelect = 0;

    public List<ShopItemSave> Skins;
}

public class ShopItemSave 
{
    public int Id;
    public bool IsBought;

    [Header("Ad")]
    public bool IsAd;
    public int MaxCountAd;
    public int CurrentCountAd;
}