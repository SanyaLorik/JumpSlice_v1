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
        foreach (ShopItemSave skin in _shopSave.Skins)
        {
            ShopItemButton button = _buttons.FirstOrDefault(i => i.Id == skin.Id);
            if (button == default)
                continue;

            if (skin.IsBought == true)
            {
                button.ActiveSelectButton();

                if (skin.Id == _shopSave.IdSelect)
                    button.UninteracSelectButton();
                else
                    button.InteractSelectButton();
            }
            else
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
                        button.UninteracSelectButton();
                }
            }
        }
    }
}