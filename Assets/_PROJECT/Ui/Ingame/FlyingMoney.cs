using TMPro;
using UnityEngine;

public class FlyingMoney : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyText;
    [field: SerializeField] public CanvasGroup CanvasGroup { get; private set; }

    public void SetText(int money)
    {
        _moneyText.text = money.ToString();
    }
}