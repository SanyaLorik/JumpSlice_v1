using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class IngameState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _ingameWindow;

    [Header("States")]
    [SerializeField] private StateBase _gameOverState;

    [Header("Gameplay")]
    [SerializeField] private PlatfromGenerator _generator;
    [SerializeField] private Movement _movement;
    [SerializeField] private PlayerSlicer _playerSlicer;
    [SerializeField] private PlayerBuilder _playerBuilder;

    [Header("Award")]
    [SerializeField] private IngameResourse _ingameResours;

    [Header("Camera")]
    [SerializeField] private CameraGameplay _cameraGameplay;

    [Header("MiniTutorial")]
    [SerializeField] private MiniTutorial _miniTutorial;

    [Inject] private IInputPlayer _playerInput;
    [Inject] private IInputActivity _inputActivity;

    private Platform _platform = null;

    private void OnEnable()
    {
        _playerInput.OnJumped += OnMove;
        _movement.OnMoved += OnNext;
    }

    private void OnDisable()
    {
        _playerInput.OnJumped -= OnMove;
        _movement.OnMoved -= OnNext;
    }

    public override async UniTask EnterAsync()
    {
        _inputActivity.Disable();

        _ingameResours.ResetResourse();

        _miniTutorial.StartTutorial();

        await _ingameWindow.ShowAsync();

        NextAsync().Forget();

        _inputActivity.Enable();
    }

    public override async UniTask ExitAsync()
    {
        _inputActivity.Disable();

        await _ingameWindow.HideAsync();
        await _gameOverState.EnterAsync();
    }

    private void OnMove()
    {
        Move();
    }

    private void OnNext()
    {
        NextAsync().Forget();
    }

    private void Move()
    {
        if (_movement.IsMoving == true)
            return;

        _movement.HideTrajectory();
        _movement.Move();
        _miniTutorial.StopTutorial();
    }

    private async UniTask NextAsync()
    {
        _inputActivity.Disable();

        if (_platform != null)
        {
            bool isExpired = await _playerSlicer.SmoothFitToPlatformAsync(_platform.SlicePattern);
            _movement.OffsetPlayer();

            if (isExpired == true)
            {
                _playerBuilder.Lose();

                await ExitAsync();

                return;
            }

            _ingameResours.AddMoney();
            _ingameResours.AddPlatform();

            if (_platform.HasBonus == true)
                await _platform.ApplyBonusAsync();
        }

        Platform platform = _generator.Generate();
        _movement.SetTarget(platform.Target.position, platform.Direction);

        if (_platform == null)
            _platform = _generator.InitalPlatform;

        await _cameraGameplay.LookAtAsync(_platform.Target.position, platform.Direction);
        await platform.AppearanceAnimationAsync();

        if (platform.HasBonus == true)
            await platform.ShowBonusAsync();

        _movement.ShowTrajectory();

        _inputActivity.Enable();

        _platform = platform;
    }
}