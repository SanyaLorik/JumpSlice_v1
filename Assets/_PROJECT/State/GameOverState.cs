using Architecture_M;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class GameOverState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _gameOverWindow;

    public override async UniTask Enter()
    {
        await _gameOverWindow.Show();
    }

    public override UniTask Exit()
    {
        throw new NotImplementedException();
    }
}