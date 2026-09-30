using SanyaBeerExtension;
using TMPro;
using UnityEngine;

public class UiMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private TextMeshProUGUI[] _moneyTexts;

    public void SetRecord(int record)
    {
        _recordText.text = record.ToString();
    }

    public void SetMoney(int money)
    {
        _moneyTexts.ForEach(i => i.text = money.ToString());
    }
}