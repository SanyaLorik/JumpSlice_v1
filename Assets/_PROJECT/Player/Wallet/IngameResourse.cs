using UnityEngine;

public class IngameResourse : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private UiIngame _uiIngame;

    [Header("Stats")]
    [SerializeField] private Wallet _money;
    [SerializeField] private Wallet _record;

    public void AddMoney()
    {
        _money.Add(1);
        _uiIngame.SetMoney(_money.Count);
    }

    public void AddPlatform()
    {
        _record.Add(1);
        _uiIngame.SetRecord(_record.Count);
    }

    public void ResetResourse()
    {
        _uiIngame.SetMoney(0);
        _uiIngame.SetRecord(0);
    }
}