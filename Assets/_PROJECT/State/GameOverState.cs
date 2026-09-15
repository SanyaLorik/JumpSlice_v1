using Architecture_M;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class GameOverState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _gameOverWindow;

    [Header("Animation")]
    [SerializeField] private DOTweenAnimationGenericBase<CanvasGroup> _continueAnimtion;

    public override async UniTask Enter()
    {
        _continueAnimtion.ResetToInitialState();

        await _gameOverWindow.Show();

        _continueAnimtion.Animate();
    }

    public override UniTask Exit()
    {
        _continueAnimtion.ResetToInitialState();
        throw new NotImplementedException();
    }
}