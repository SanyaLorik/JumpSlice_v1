using System;
using UnityEngine;

[Serializable]
public class GameData : GameDataBase
{
    [Header("Profit")]
    public int StartProfitPrice = 10;
    public int StartProfitIncoming = 1;

    [Header("Кривые приращения дохода")]
    [Tooltip("Базовое приращение дохода за уровень")]
    public int IncomeBaseStep = 1;
    [Tooltip("Доп. приращение, растущее с уровнем (замедляющееся)")]
    public float IncomeBonusFactor = 3f;

    [Header("Кривые приращения цены")]
    public float PricePower = 1.6f;
    public float PriceExponent = 1.05f;

    [Header("ProfitTest")]
    public int ProfitLevelTest;

    [Header("Health")]
    public int HealthPrice = 25;

    public void ProfitTest()
    {
        Debug.Log($"GetProfitIncoming: {GetProfitIncoming(ProfitLevelTest)}\n" +
            $"GetProfitPrice: {GetProfitPrice(ProfitLevelTest)}");
    }

    public int GetProfitIncoming(int level)
    {
        level = Mathf.Max(1, level);

        // Сумма приращений: step(k) = IncomeBaseStep + log(k)
        // Сумма >= level * IncomeBaseStep => строго растёт
        float income = StartProfitIncoming + (level - 1) * IncomeBaseStep;

        // Логарифмический бонус сверху — он тоже монотонно растёт
        for (int k = 2; k <= level; k++)
            income += Mathf.Log(k) * IncomeBonusFactor;

        return Mathf.RoundToInt(income);
    }

    public int GetProfitPrice(int level)
    {
        level = Mathf.Max(1, level);
        if (level == 1) return StartProfitPrice;

        float price = StartProfitPrice
                    * Mathf.Pow(level, PricePower)
                    * Mathf.Pow(PriceExponent, level - 1);

        return Mathf.RoundToInt(price);
    }
}