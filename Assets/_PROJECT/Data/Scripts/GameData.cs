using System;
using UnityEngine;

[Serializable]
public class GameData : GameDataBase
{
    public void GetMoneyForPlatform(int levelId)
    {
        levelId = Math.Max(levelId - 1, 0);
    }
}