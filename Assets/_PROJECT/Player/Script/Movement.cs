using Cysharp.Threading.Tasks;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _target;

    [Header("Input")]
    [SerializeField] private CustomInput _input;

    [Header("Move")]
    [SerializeField] private AnimationCurve _trajectory;
    [SerializeField] private float _duration;
    [SerializeField] private float _height;

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
        Move(_target).Forget();
    }

    private async UniTaskVoid Move(Transform target)
    {
        float expendedTime = 0;

        Vector3 initial = transform.position;
        Vector3 final = target.position;

        do
        {
            // Нормализованное время (0..1)
            float t = Mathf.Clamp01(expendedTime / _duration);

            Vector3 horizontalPosition = Vector3.Lerp(initial, final, t);

            // Вертикальное движение по кривой
            float heightOffset = _trajectory.Evaluate(t) * _height;
            Vector3 verticalOffset = Vector3.up * heightOffset;

            // Финальная позиция
            transform.position = horizontalPosition + verticalOffset;

            expendedTime += Time.deltaTime;
            await UniTask.Yield();
        }
        while (expendedTime < _duration);
    }
}