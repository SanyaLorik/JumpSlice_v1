using Architecture_M;
using MediaKit_M.SkinChanger;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopItemButton[] _buttons;
    [SerializeField] private Wallet _globalWallet;

    [Inject] private IAdvertisingMonetization _advertising;
    [Inject] private IGameSave _gameSave;

    private ShopSave _shopSave;

    private void Awake()
    {
        _shopSave = _gameSave.GetSave<GameSave>().Shop;
    }

    private void Start()
    {
        foreach (var button in _buttons)
        {
            button.AddBuyClickCallback(() => BuySkin(button));
            button.AddSelectClickCallback(() => SelectSkin(button));
            button.AddAdClickCallback(() => WatchAdSkin(button));
        }
    }

    public void UpdateView()
    {
        UpdateBoughtSkin();
    }

    private void UpdateBoughtSkin()
    {
        foreach (ShopItemButton button in _buttons)
        {
            ShopItemSave skinSave = _shopSave.Skins.FirstOrDefault(i => i.Id == button.Id);
            if (skinSave == default || skinSave == null)
            {
                AddSkinToSave(button);
                continue;
            }

            if (skinSave.IsBought == true)
            {
                button.ActiveSelectButton();

                if (button.Id == _shopSave.IdSelect)
                    button.UninteracSelectButton();
                else
                    button.InteractSelectButton();
            }
            else
            {
                ControlNonBought(button);
            }
        }
    }

    private void AddSkinToSave(ShopItemButton button)
    {
        ShopItemSave shopItem = new()
        {
            Id = button.Id,
            IsBought = false,
            IsAd = button.IsAd,
            MaxCountAd = button.MaxCountAd
        };

        _shopSave.Skins.Add(shopItem);

        ControlNonBought(button);

        _gameSave.Save();
    }

    private void ControlNonBought(ShopItemButton button)
    {
        if (button.IsAd == true)
        {
            button.ActiveAdButton();

            ShopItemSave skinSave = _shopSave.Skins.FirstOrDefault(i => i.Id == button.Id);
            if (skinSave == default || skinSave == null)
                return;

            button.SetAdText(skinSave.CurrentCountAd);
        }
        else
        {
            button.ActiveBuyButton();

            if (button.Price <= _globalWallet.Count)
                button.InteractBuyButton();
            else
                button.UninteractBuyButton();
        }
    }

    private void BuySkin(ShopItemButton button)
    {
        if (button.Price > _globalWallet.Count)
            return;

        SelectSkin(button);

        _gameSave.Save();
    }

    private void SelectSkin(ShopItemButton button)
    {
        _shopSave.IdSelect = button.Id;

        AddSkinAsBought(button);
        UpdateBoughtSkin();
        ApplySkin(button);

        _gameSave.Save();

    }

    private void WatchAdSkin(ShopItemButton button)
    {
        _advertising.InvokeRewarded(
            null,
            (isSuccess) =>
            {
                if (isSuccess == false)
                    return;

                ShopItemSave skinSave = _shopSave.Skins.FirstOrDefault(i => i.Id == button.Id);
                if (skinSave == default || skinSave == null)
                    return;

                skinSave.CurrentCountAd++;
                if (skinSave.CurrentCountAd >= button.MaxCountAd)
                {
                    SelectSkin(button);
                    return;
                }

                button.SetAdText(skinSave.CurrentCountAd);
            });
    }

    private void AddSkinAsBought(ShopItemButton button)
    {
        ShopItemSave skinSave = _shopSave.Skins.FirstOrDefault(i => i.Id == button.Id);
        if (skinSave != default || skinSave != null)
        {
            skinSave.IsBought = true;
            return;
        }

        ShopItemSave shopItem = new()
        {
            Id = button.Id,
            IsBought = true
        };

        _shopSave.Skins.Add(shopItem);
    }

    private void ApplySkin(ShopItemButton button)
    {

    }
}