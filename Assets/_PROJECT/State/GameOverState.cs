using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameOverState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _gameOverWindow;

    [Header("Ui")]
    [SerializeField] private UiGameOver _uiGameOver;

    [Header("Managment")]
    [SerializeField] private Button _continueButton;

    [Header("States")]
    [SerializeField] private StateBase _menuState;

    [Header("Wallets")]
    [SerializeField] private Wallet _ingameMoneyWallet;
    [SerializeField] private Wallet _ingameRecordWallet;

    [Header("Animation")]
    [SerializeField] private DOTweenAnimationGenericBase<CanvasGroup> _continueAnimtion;

    [Header("Resourse")]
    [SerializeField] private MenuResource _menuResource;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(OnContinue);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(OnContinue);
    }

    public override async UniTask EnterAsync()
    {
        _continueAnimtion.ResetToInitialState();

        _uiGameOver.SetMoney(_ingameMoneyWallet.Count);
        _uiGameOver.SetRecord(_ingameRecordWallet.Count);

        await _gameOverWindow.ShowAsync();

        _continueAnimtion.Animate();
    }

    public override async UniTask ExitAsync()
    {
        _continueAnimtion.ResetToInitialState();
        _menuResource.UpdateResourse();

        await _gameOverWindow.HideAsync();
    }

    private void OnContinue()
    {
        ToMenu().Forget();
    }

    private async UniTaskVoid ToMenu()
    {
        await ExitAsync();
        await _menuState.EnterAsync();
    }
}
