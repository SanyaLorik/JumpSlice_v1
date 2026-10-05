using Architecture_M;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOverState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _gameOverWindow;

    [Header("Ui")]
    [SerializeField] private UiGameOver _uiGameOver;

    [Header("States")]
    [SerializeField] private StateBase _menuState;

    [Header("Wallets")]
    [SerializeField] private Wallet _ingameMoneyWallet;
    [SerializeField] private Wallet _ingameRecordWallet;

    [Header("Animation")]
    [SerializeField] private DOTweenAnimationGenericBase<CanvasGroup> _continueAnimtion;

    [Header("Resourse")]
    [SerializeField] private MenuResource _menuResource;

    [Inject] private IAdvertisingMonetization _monetization;

    private void Start()
    {
        _uiGameOver.AddContinueButtonListner(OnContinue);
        _uiGameOver.AddX2AdButtonListner(OnX2Ad);
    }

    public override async UniTask EnterAsync()
    {
        _continueAnimtion.ResetToInitialState();

        _uiGameOver.SetMoney(_ingameMoneyWallet.Count);
        _uiGameOver.SetX2AdMoney(_ingameMoneyWallet.Count * 2);
        _uiGameOver.SetRecord(_ingameRecordWallet.Count);

        await _gameOverWindow.ShowAsync();

        _continueAnimtion.Animate();
    }

    public override async UniTask ExitAsync()
    {
        _continueAnimtion.ResetToInitialState();
        _menuResource.UpdateResourseAfterGameOver();

        await _gameOverWindow.HideAsync();
    }

    private void OnContinue()
    {
        ToMenu().Forget();
    }

    private void OnX2Ad()
    {
        _monetization.InvokeRewarded(null,
            (isSuccess) =>
            {
                if (isSuccess == true)
                {
                    _ingameMoneyWallet.Add(_ingameMoneyWallet.Count);
                    ToMenu().Forget();
                }
            });
    }

    private async UniTaskVoid ToMenu()
    {
        await ExitAsync();
        await _menuState.EnterAsync();
    }
}