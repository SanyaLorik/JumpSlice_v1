using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform _player;

    [Header("Input")]
    [SerializeField] private CustomInput _input;

    [Header("Move")]
    [SerializeField] private TrajectoryLine _trajectory;
    [SerializeField] private AnimationCurve _verticalCurve;
    [SerializeField] private AnimationCurve _horizantalCurve;
    [SerializeField] private float _duration;

    public event Action OnMoved;

    public Transform Target { private get; set; }

    private bool _isMoving = false;

    private void OnEnable()
    {
        _input.OnUp += OnMove;
    }

    private void OnDisable()
    {
        _input.OnUp -= OnMove;
    }

    private void OnMove()
    {
        if (_isMoving == true) 
            return;

        _trajectory.HideAnimationAsync().Forget();
        MoveAsync(Target).Forget();
    }

    private async UniTaskVoid MoveAsync(Transform target)
    {
        _isMoving = true;

        float expendedTime = 0;

        Vector3 initial = transform.position;
        Vector3 final = target.position;

        do
        {
            // Нормализованное время (0..1)
            float t = Mathf.Clamp01(expendedTime / _duration);

            // Горизонтальное движение по кривой
            float horizontalLerp = _horizantalCurve.Evaluate(t);
            Vector3 horizontalPosition = Vector3.Lerp(initial, final, horizontalLerp);

            // Вертикальное движение по кривой
            float verticalLerp = _verticalCurve.Evaluate(t);
            float heightOffset = _trajectory.Trajectory.Evaluate(verticalLerp) * _trajectory.Height;
            Vector3 verticalOffset = Vector3.up * heightOffset;

            // Финальная позиция
            transform.position = horizontalPosition + verticalOffset;

            expendedTime += Time.deltaTime;
            await UniTask.Yield();
        }
        while (expendedTime < _duration);

        _isMoving = false;

        OnMoved?.Invoke();
    }
}

