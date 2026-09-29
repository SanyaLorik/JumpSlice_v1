using Architecture_M;
using System.Linq;
using UnityEngine;
using Zenject;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopItemButton[] _buttons;
    [SerializeField] private Wallet _globalWallet;

    [Inject] private IGameSave _gameSave;

    private ShopSave _shopSave;

    private void Awake()
    {
        _shopSave = _gameSave.GetSave<GameSave>().Shop;
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
            IsAd = button.IsAd,
            MaxCountAd = button.MaxCountAd
        };

        _shopSave.Skins.Add(shopItem);

        ControlNonBought(button);
    }

    private void ControlNonBought(ShopItemButton button)
    {
        if (button.IsAd == true)
        {
            button.ActiveAdButton();
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
}