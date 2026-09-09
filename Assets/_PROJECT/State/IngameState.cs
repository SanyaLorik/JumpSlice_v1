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

    [Inject] private IInputPlayer _input;

    private void OnEnable()
    {
        _input.OnJumped += OnMove;
        _movement.OnMoved += OnNext;
    }

    private void OnDisable()
    {
        _input.OnJumped -= OnMove;
        _movement.OnMoved -= OnNext;
    }

    public override async UniTask Enter()
    {
        _input.Disable();

        await _ingameWindow.Show();

        Next();

        _input.Enable();
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
        _movement.Move();
    }

    private void Next()
    {
        Platform platform = _generator.Generate();
        _movement.SetTarget(platform.Target.position, platform.Direction);
    }
}
