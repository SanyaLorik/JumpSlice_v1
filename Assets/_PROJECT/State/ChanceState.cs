using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChanceState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _chanceWindow;

    [Header("Ui")]
    [SerializeField] private UiChance _uiChance;

    [Header("States")]
    [SerializeField] private StateBase _ingameState;
    [SerializeField] private StateBase _gameOverState;

    [Header("Animation")]
    [SerializeField] private DOTweenAnimationGenericBase<CanvasGroup> _skipAnimtion;

    [Inject] private IAdvertisingMonetization _monetization;
    [Inject] private IGameSave _gameSave;
    private GameSave _save;

    private void Awake()
    {
        _save = _gameSave.GetSave<GameSave>();
    }

    private void Start()
    {
        _uiChance.AddReturnButtonListner(OnReturn);
        _uiChance.AddReturnAdButtonListner(OnReturnAd);
        _uiChance.AddSkipButtonListner(OnSkip);
    }

    public override async UniTask EnterAsync()
    {
        if (_save.HealthCount > 0)
            _uiChance.InteractReturnButton();
        else
            _uiChance.UninteractReturnButton();

        _skipAnimtion.ResetToInitialState();

        _uiChance.SetHealth(_save.HealthCount);

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
        _save.HealthCount--;
        _gameSave.Save();

        ToIngame().Forget();
    }

    private void OnReturnAd()
    {
        _monetization.InvokeRewarded(null,
            (isSuccess) =>
            {
                if (isSuccess == true)
                    ToIngame().Forget();
            });
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