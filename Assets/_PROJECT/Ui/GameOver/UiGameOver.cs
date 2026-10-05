using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _x2AdMoneyText;
    [SerializeField] private Button _x2AdButton;
    [SerializeField] private Button _continueButton;

    public void SetRecord(int record)
    {
        _recordText.text = record.ToString();
    }

    public void SetMoney(int money)
    {
        _moneyText.text = money.ToString();
    }

    public void SetX2AdMoney(int money)
    {
        _x2AdMoneyText.text = money.ToString();
    }

    public void AddContinueButtonListner(Action action)
    {
        _continueButton.onClick.AddListener(action.Invoke);
    }

    public void AddX2AdButtonListner(Action action)
    {
        _x2AdButton.onClick.AddListener(action.Invoke);
    }
}
