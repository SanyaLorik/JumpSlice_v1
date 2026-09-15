using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameOverState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _gameOverWindow;

    [Header("Managment")]
    [SerializeField] private Button _continueButton;

    [Header("States")]
    [SerializeField] private StateBase _menuState;

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

    public override async UniTask Enter()
    {
        _continueAnimtion.ResetToInitialState();

        await _gameOverWindow.Show();

        _continueAnimtion.Animate();
    }

    public override async UniTask Exit()
    {
        _continueAnimtion.ResetToInitialState();

        await _gameOverWindow.Hide();
    }

    private void OnContinue()
    {
        ToMenu().Forget();
    }

    private async UniTaskVoid ToMenu()
    {
        await Exit();
        await _menuState.Enter();
    }
}