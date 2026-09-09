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

    [Header("Camera")]
    [SerializeField] private CameraGameplay _cameraGameplay;

    [Inject] private IInputPlayer _playerInput;
    [Inject] private IInputActivity _inputActivity;

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

        Next();

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
        Next();
    }

    private void Move()
    {
        if (_movement.IsMoving == false)
            _movement.Move();
    }

    private void Next()
    {
        Platform platform = _generator.Generate();
        _movement.SetTarget(platform.Target.position, platform.Direction);
        _cameraGameplay.LookAt(platform.Direction).Forget();
    }
}
