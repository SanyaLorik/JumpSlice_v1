using Cysharp.Threading.Tasks;
using SanyaBeerExtension;
using System;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _player;
    [SerializeField] private AnimationCurve _trajectory;
    [SerializeField] private float _height;

    [Header("Target")]
    [SerializeField] private AnimationCurve _lineEase;
    [SerializeField] private PairedValue<float> _range;
    [SerializeField] private float _durationRange;

    [Header("Animation")]
    [SerializeField] private MovementAnimation _movementAnimation;
    [SerializeField] private TrajectoryAnimation _trajectoryAnimation;

    public event Action OnMoved;

    private CancellationTokenSource _tokenSource;
    private Vector3 _currentTarget;

    private UniTask _movingTask;

    public bool IsMoving => _movingTask.Status.IsCompleted() == false;


    [ContextMenu("Create")]
    private void CreateInInspector()
    {
        _trajectoryAnimation.CreateLineDebug(_player.position, _trajectory, _height);
    }

    public void SetTarget(Vector3 target, Vector3 direction)
    {
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();

        _tokenSource = new CancellationTokenSource();

        _trajectoryAnimation.ShowAnimationAsync().Forget();

        MoveTargetAsync(target, direction).Forget();
    }

    public void Move()
    {
        _trajectoryAnimation.HideAnimationAsync().Forget();

        _movingTask = _movementAnimation
            .MoveAsync(_player, _currentTarget, _trajectory, _height)
            .ContinueWith(() => OnMoved?.Invoke());
    }

    private async UniTaskVoid MoveTargetAsync(Vector3 target, Vector3 direction)
    {
        while (_tokenSource.IsCancellationRequested == false)
        {
            Vector3 from = target + direction * _range.From;
            Vector3 to = target + direction * _range.To;

            await MoveLocal(from, to);
            await MoveLocal(to, from);
        }
    }

    private async UniTask MoveLocal(Vector3 from, Vector3 to)
    {
        float expendedTime = 0;

        do
        {
            // Нормализованное время (0..1)
            float t = Mathf.Clamp01(expendedTime / _durationRange);

            float lerp = _lineEase.Evaluate(t);

            _currentTarget = Vector3.Lerp(from, to, lerp);

            await UniTask.WaitWhile(() => IsMoving == true, cancellationToken: _tokenSource.Token);

            _trajectoryAnimation.CreateLine(_currentTarget, _trajectory, _height);

            expendedTime += Time.deltaTime;

            await UniTask.Yield(cancellationToken: _tokenSource.Token);
        }
        while (expendedTime < _durationRange && _tokenSource.IsCancellationRequested == false);
    }
}