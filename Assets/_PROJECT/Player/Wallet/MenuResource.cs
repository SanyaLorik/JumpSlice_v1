using Architecture_M;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuResource : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private UiMenu _uiMenu;

    [Header("Wallets")]
    [SerializeField] private Wallet _ingameMoneyWallet;
    [SerializeField] private Wallet _ingameRecordWallet;
    [SerializeField] private Wallet _menuMoneyWallet;
    [SerializeField] private Wallet _menuRecordWallet;

    [Inject] private IGameSave _gameSave;
    [Inject] private GameData _data;
    private GameSave _save;

    private void Awake()
    {
        _save = _gameSave.GetSave<GameSave>();
    }

    private void Start()
    {
        _menuMoneyWallet.Add(_save.Money);
        _menuRecordWallet.Add(_save.Record);

        _uiMenu.AddProfitButtonListner(OnBuyProfit);
        _uiMenu.AddHealthButtonListner(OnBuyHealth);

        UpdateBonus();
    }

    private void OnEnable()
    {
        _menuMoneyWallet.OnChanged += OnUpdateViewMoney;
        _menuRecordWallet.OnChanged += OnUpdateRecord;
    }

    private void OnDisable()
    {
        _menuMoneyWallet.OnChanged -= OnUpdateViewMoney;
        _menuRecordWallet.OnChanged -= OnUpdateRecord;
    }

    public void UpdateResourseAfterGameOver()
    {
        _menuMoneyWallet.Add(_ingameMoneyWallet.Count);

        _save.Record = Math.Max(_save.Record, _ingameRecordWallet.Count);
        _menuRecordWallet.Set(_save.Record);

        UpdateBonus();

        _gameSave.Save();
    }

    private void OnUpdateViewMoney(int value)
    {
        _uiMenu.SetMoney(value);
    }

    private void OnUpdateRecord(int value)
    {
        _uiMenu.SetRecord(value);
    }

    private void OnBuyProfit()
    {
        int price = _data.GetProfitPrice(_save.ProfitLevel);
        if (_save.Money < _data.GetProfitPrice(_save.ProfitLevel))
            return;

        _save.Money -= price;
        _save.ProfitLevel++;

        _menuMoneyWallet.Remove(price);

        UpdateBonus();
        _gameSave.Save();
    }

    private void OnBuyHealth()
    {
        int price = _data.HealthPrice;
        if (_save.Money < _data.HealthPrice)
            return;

        _save.Money -= price;
        _save.HealthCount++;

        _menuMoneyWallet.Remove(price);

        UpdateBonus();
        _gameSave.Save();
    }

    private void UpdateBonus()
    {
        _uiMenu.SetHealthCountText(_save.HealthCount);

        _uiMenu.SetProfitLevelText(_save.ProfitLevel);
        _uiMenu.SetProfitPriceText(_data.GetProfitPrice(_save.ProfitLevel));
        _uiMenu.SetProfitIncomigText(_data.GetProfitIncoming(_save.ProfitLevel));

        if (_save.Money >= _data.GetProfitPrice(_save.ProfitLevel))
            _uiMenu.InteractProfitButton();
        else
            _uiMenu.UninteractProfitButton();

        if (_save.Money >= _data.HealthPrice)
            _uiMenu.InteractHealthButton();
        else
            _uiMenu.UninteractHealthButton();
    }
}