using SanyaBeerExtension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    [field: SerializeField] public int Id { get; private set; }
    [field: SerializeField] public bool IsAd { get; private set; }
    [field: SerializeField] public int Price { get; private set; }

    [Header("Buy")]
    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _buyImage;
    [SerializeField] private TextMeshProUGUI _moneyText;

    [Header("Selected")]
    [SerializeField] private Button _selectedButton;
    [SerializeField] private Image _selectedImage;

    [Header("Ad")]
    [SerializeField] private Button _adButton;
    [SerializeField] private TextMeshProUGUI _adText;

    private void Awake()
    {
        _moneyText.text = Price.ToString();
    }

    public void ActiveBuyButton()
    {
        _buyButton.ActiveSelf();
        _selectedButton.DisactiveSelf();
        _adButton.DisactiveSelf();
    }

    public void ActiveSelectButton()
    {
        _buyButton.DisactiveSelf();
        _selectedButton.ActiveSelf();
        _adButton.DisactiveSelf();
    }

    public void InteractBuyButton()
    {
        _buyButton.interactable = true;
    }

    public void UninteractBuyButton()
    {
        _buyButton.interactable = false;
    }

    public void InteractSelectButton()
    {
        _selectedButton.interactable = true;
    }

    public void UninteracSelectButton()
    {
        _selectedButton.interactable = false;
    }
    
    public void ActiveAdButton()
    {
        _buyButton.DisactiveSelf();
        _selectedButton.DisactiveSelf();
        _adButton.ActiveSelf();
    }

    public void SetAdText(int adCount)
    {
        _adText.text = adCount.ToString();
    }
}