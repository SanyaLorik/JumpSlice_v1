using Architecture_M;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class IngameState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _ingameWindow;

    [Header("Gameplay")]
    [SerializeField] private PlatfromGenerator _generator;
    [SerializeField] private Movement _movement;
    [SerializeField] private PlayerSlicer _playerSlicer;

    [Header("Camera")]
    [SerializeField] private CameraGameplay _cameraGameplay;

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

        await _ingameWindow.Show();

        NextAsync().Forget();

        _inputActivity.Enable();
    }

    public override UniTask Exit()
    {
        throw new();
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
        if (_movement.IsMoving == false)
            _movement.Move();
    }

    private async UniTask NextAsync()
    {
        _inputActivity.Disable();

        if (_platform != null)
        {
            await _playerSlicer.SmoothFitToPlatformAsync(_platform.SlicePattern);

            if (_platform.HasBonus == true)
                await _platform.ApplyAsync();
        }

        Platform platform = _generator.Generate();
        _movement.SetTarget(platform.Target.position, platform.Direction);
        await _cameraGameplay.LookAt(platform.Direction);

        _inputActivity.Enable();

        _platform = platform;
    }
}
