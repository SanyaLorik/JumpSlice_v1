using SanyaBeerExtension;
using TMPro;
using UnityEngine;

public class UiMenu : MonoBehaviour
{
    [Header("Header Stats")]
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private TextMeshProUGUI[] _moneyTexts;

    [Header("Bonus")]
    [Header("Profit")]
    [SerializeField] private TextMeshProUGUI _profitLevelText;
    [SerializeField] private TextMeshProUGUI _profitIncomingText;
    [SerializeField] private TextMeshProUGUI _profitPriceText;

    [Header("Health")]
    [SerializeField] private TextMeshProUGUI _healthCountText;
    [SerializeField] private TextMeshProUGUI _healthPriceText;

    public void SetRecord(int record)
    {
        _recordText.text = record.ToString();
    }

    public void SetMoney(int money)
    {
        _moneyTexts.ForEach(i => i.text = money.ToString());
    }

    // profit
    public void SetProfitLevelText(int profitLevel)
    {
        _profitLevelText.text = profitLevel.ToString();
    }

    public void SetProfitIncomigText(int profitIncoming)
    {
        _profitIncomingText.text = profitIncoming.ToString();
    }

    public void SetProfitPriceText(int profitPrice)
    {
        _profitPriceText.text = profitPrice.ToString();
    }

    // health
    public void SetHealthCountText(int healthCount)
    {
        _healthCountText.text = healthCount.ToString();
    }

    public void SetHealthPriceText(int healthPrice)
    {
        _healthPriceText.text = healthPrice.ToString();
    }
}