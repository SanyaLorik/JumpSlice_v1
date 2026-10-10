using Cysharp.Threading.Tasks;
using SanyaBeerExtension;
using System;
using System.Threading;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _slicedCube;
    [SerializeField] private AnimationCurve _trajectory;
    [SerializeField] private float _height;

    [Header("Target")]
    [SerializeField] private AnimationCurve _lineEase;
    [SerializeField] private PairedValue<float> _range;
    [SerializeField] private float _durationRange;
    [SerializeField] private float _numeratorBase;
    [SerializeField] private float _denominatorBase;
    [SerializeField] private float _increment;

    [Header("Animation")]
    [SerializeField] private MovementAnimation _movementAnimation;
    [SerializeField] private TrajectoryAnimation _trajectoryAnimation;

    [Header("Effect")]
    [SerializeField] private PlayerEffect _effectFx;

    public event Action OnMoved;

    private CancellationTokenSource _tokenSource;
    private Vector3 _currentTarget;

    private UniTask _movingTask;

    private int _counter = -1;
    private float _currentDuration = 0;

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

        target = OffsetTarget(target, direction);

        MoveTargetAsync(target, direction).Forget();
    }

    public void Move()
    {
        _effectFx.Jump();
        AudioPlayerManager.Instance.Play(AudioClipType.Jump);

        _movingTask = _movementAnimation
            .MoveAsync(_player, _currentTarget, _trajectory, _height)
            .ContinueWith(() =>
            {
                OnMoved?.Invoke();

                _effectFx.Fall();
                AudioPlayerManager.Instance.Play(AudioClipType.Fall);
            });
    }

    public void OffsetPlayer()
    {
        _player.localPosition += _slicedCube.localPosition;
        _slicedCube.localPosition = Vector3.zero;
    }
    
    public void ShowTrajectory()
    {
        _trajectoryAnimation.ShowAnimationAsync().Forget();
    }

    public void HideTrajectory()
    {
        _trajectoryAnimation.HideAnimationAsync().Forget();

    }

    public void IncrimentCounter()
    {
        _counter++;
        float p = ((_counter * _increment) + _numeratorBase) / ((_counter * _increment) + _denominatorBase);
        _currentDuration = _durationRange - Mathf.Pow(_durationRange, p);
    }

    public void ResetCounter()
    {
        _counter = -1;
    }

    private async UniTaskVoid MoveTargetAsync(Vector3 target, Vector3 direction)
    {
        while (_tokenSource.IsCancellationRequested == false)
        {
            Vector3 from = target + direction * _range.From;
            Vector3 to = target + direction * _range.To;

            await MoveLocalAync(from, to);
            await MoveLocalAync(to, from);
        }
    }

    private async UniTask MoveLocalAync(Vector3 from, Vector3 to)
    {
        float expendedTime = 0;

        do
        {
            // Нормализованное время (0..1)
            float t = Mathf.Clamp01(expendedTime / _currentDuration);

            float lerp = _lineEase.Evaluate(t);

            _currentTarget = Vector3.Lerp(from, to, lerp);

            await UniTask.WaitWhile(() => IsMoving == true, cancellationToken: _tokenSource.Token);

            _trajectoryAnimation.CreateLine(_currentTarget, _trajectory, _height);

            expendedTime += Time.deltaTime;

            await UniTask.Yield(cancellationToken: _tokenSource.Token);
        }
        while (expendedTime <= _currentDuration && _tokenSource.IsCancellationRequested == false);
    }

    private Vector3 OffsetTarget(Vector3 target, Vector3 direction)
    {
        if (direction == Vector3.forward)
            target.x = _player.position.x;
        else if (direction == Vector3.left)
            target.z = _player.position.z;

        return target;
    }
}