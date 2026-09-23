using Architecture_M;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ChanceState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _chanceWindow;

    [Header("Ui")]
    [SerializeField] private UiChance _uiChance;

    [Header("Managment")]
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _skipButton;

    [Header("States")]
    [SerializeField] private StateBase _ingameState;
    [SerializeField] private StateBase _gameOverState;

    [Header("Animation")]
    [SerializeField] private DOTweenAnimationGenericBase<CanvasGroup> _skipAnimtion;

    private void OnEnable()
    {
        _returnButton.onClick.AddListener(OnReturn);
        _skipButton.onClick.AddListener(OnSkip);
    }

    private void OnDisable()
    {
        _returnButton.onClick.RemoveListener(OnReturn);
        _skipButton.onClick.RemoveListener(OnSkip);
    }

    public override async UniTask EnterAsync()
    {
        _skipAnimtion.ResetToInitialState();

        _uiChance.SetHealth(1488);

        await _chanceWindow.ShowAsync();

        _skipAnimtion.Animate();
    }

    public override async UniTask ExitAsync()
    {
        _skipAnimtion.ResetToInitialState();

        await _chanceWindow.HideAsync();
    }

    private void OnReturn()
    {
        ToIngame().Forget();
    }

    private void OnSkip()
    {
        ToGameOver().Forget();
    }

    private async UniTaskVoid ToIngame()
    {
        await ExitAsync();
        await (_ingameState as IngameState).EnterReturnAsync();
    }

    private async UniTaskVoid ToGameOver()
    {
        await ExitAsync();
        await _gameOverState.EnterAsync();
    }
}