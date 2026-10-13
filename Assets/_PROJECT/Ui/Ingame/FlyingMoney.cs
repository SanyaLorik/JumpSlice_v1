using TMPro;
using UnityEngine;

public class FlyingMoney : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    public int Money { get; private set; } = -1;

    public void SetText(int money)
    {
        Money = money;
        _moneyText.text = money.ToString();
    }
}