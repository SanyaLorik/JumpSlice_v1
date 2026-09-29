using Architecture_M;
using System;
using UnityEngine;
using Zenject;

public class MenuResource : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private UiMenu _uiMenu;

    [Header("Wallet")]

    [Header("Wallets")]
    [SerializeField] private Wallet _ingameMoneyWallet;
    [SerializeField] private Wallet _ingameRecordWallet;
    [SerializeField] private Wallet _menuMoneyWallet;
    [SerializeField] private Wallet _menuRecordWallet;

    [Inject] private IGameSave _gameSave;
    private GameSave _save;

    private void Awake()
    {
        _save = _gameSave.GetSave<GameSave>();
    }

    private void Start()
    {
        _menuMoneyWallet.Add(_save.Money);
        _menuRecordWallet.Add(_save.Record);
    }

    private void OnEnable()
    {
        _menuMoneyWallet.OnChanged += OnUpdateViewMoney;
        _menuRecordWallet.OnChanged += OnRecord;
    }

    private void OnDisable()
    {
        _menuMoneyWallet.OnChanged -= OnUpdateViewMoney;
        _menuRecordWallet.OnChanged -= OnRecord;
    }

    public void UpdateResourse()
    {
        _menuMoneyWallet.Add(_ingameMoneyWallet.Count);

        _save.Record = Math.Max(_save.Record, _ingameRecordWallet.Count);
        _menuRecordWallet.Set(_save.Record);

        _gameSave.Save();
    }

    private void OnUpdateViewMoney(int value)
    {
        _uiMenu.SetMoney(value);
    }

    private void OnRecord(int value)
    {
        _uiMenu.SetRecord(value);
    }
}
