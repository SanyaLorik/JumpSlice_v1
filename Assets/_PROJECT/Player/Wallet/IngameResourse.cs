using Architecture_M;
using UnityEngine;
using Zenject;

public class IngameResourse : MonoBehaviour
{
    public static IngameResourse Instance { get; private set; }

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
        Instance = this;

        _save = _gameSave.GetSave<GameSave>();
    }

    public void AddMoney()
    {
        int incoming = _data.GetProfitIncoming(_save.ProfitLevel);

        AddMoney(incoming);
        _uiIngame.AnimateMoney(incoming);
    }

    public void AddPlatform()
    {
        _record.Add(1);
        _uiIngame.SetRecord(_record.Count);
        _uiIngame.AnimateRecord();
    }

    public void ResetResourse()
    {
        _uiIngame.SetMoney(0);
        _uiIngame.SetRecord(0);
    }

    public void AddMoney(int money)
    {
        _money.Add(money);
        _uiIngame.SetMoney(_money.Count);

        AudioPlayerManager.Instance.Play(AudioClipType.MoneyTake);
    }
}