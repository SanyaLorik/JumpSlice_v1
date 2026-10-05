using Architecture_M;
using UnityEngine;
using Zenject;

public class IngameResourse : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private UiIngame _uiIngame;

    [Header("Stats")]
    [SerializeField] private Wallet _money;
    [SerializeField] private Wallet _record;

    [Inject] private IGameSave _gameSave;
    [Inject] private GameData _data;
    private GameSave _save;

    private void Awake()
    {
        _save = _gameSave.GetSave<GameSave>();
    }

    public void AddMoney()
    {
        int incoming = _data.GetProfitIncoming(_save.ProfitLevel);

        _money.Add(incoming);
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