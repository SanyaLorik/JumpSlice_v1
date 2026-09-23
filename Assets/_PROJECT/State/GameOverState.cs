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
    [SerializeField] private Wallet _moneyWallet;
    [SerializeField] private Wallet _recordWallet;

    [Header("Animation")]
    [SerializeField] private DOTweenAnimationGenericBase<CanvasGroup> _continueAnimtion;

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

        _uiGameOver.SetMoney(_moneyWallet.Count);
        _uiGameOver.SetRecord(_recordWallet.Count);

        await _gameOverWindow.ShowAsync();

        _continueAnimtion.Animate();
    }

    public override async UniTask ExitAsync()
    {
        _continueAnimtion.ResetToInitialState();

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
