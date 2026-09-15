using Architecture_M;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
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

    [Header("Award")]
    [SerializeField] private IngameMoney _ingameMoney;

    [Header("Camera")]
    [SerializeField] private CameraGameplay _cameraGameplay;

    [Header("MiniTutorial")]
    [SerializeField] private MiniTutorial _miniTutorial;

    [Inject] private IInputPlayer _playerInput;
    [Inject] private IInputActivity _inputActivity;

    private Platform _platform;

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

    public override async UniTask Enter()
    {
        _inputActivity.Disable();

        _miniTutorial.StartTutorial();

        await _ingameWindow.Show();

        NextAsync().Forget();

        _inputActivity.Enable();
    }

    public override async UniTask Exit()
    {
        _inputActivity.Disable();

        await _ingameWindow.Hide();
        await _gameOverState.Enter();
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
                await Exit();
                return;
            }

            _ingameMoney.AddMoney(_generator.NumberCounter);

            if (_platform.HasBonus == true)
                await _platform.ApplyAsync();
        }

        Platform platform = await _generator.Generate();
        _movement.SetTarget(platform.Target.position, platform.Direction);

        await _cameraGameplay.LookAt(platform.Direction);

        _inputActivity.Enable();

        _platform = platform;
    }
}