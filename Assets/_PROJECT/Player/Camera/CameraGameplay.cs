using Cysharp.Threading.Tasks;
using SanyaBeerExtension;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;

public class CameraGameplay : MonoBehaviour
{
    [SerializeField] private CameraGamplayPosition[] _positions;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _durationMoving = 0.32f;
    [SerializeField] private float _durationRotation = 0.32f;

    private CancellationTokenSource _tokenSource;

    public async UniTask LookAtAsync(Vector3 target, Vector3 direction)
    {
        Transform final = _positions.FirstOrDefault(i => i.Direction == direction).Position;

        Vector3 position = final.position;
        Quaternion rotation = final.rotation;

        position += target.ResetY();

        await MoveToTargetAsync(position, rotation);
    }

    public async UniTask MoveToTargetAsync(Vector3 position, Quaternion rotation)
    {
        // Отменяем предыдущую операцию
        _tokenSource?.Cancel();
        _tokenSource?.Dispose();
        _tokenSource = new CancellationTokenSource();

        try
        {
            // Запускаем передвижение и поворот параллельно
            await UniTask.WhenAll(
                MovePositionAsync(position, _tokenSource.Token),
                RotateAsync(rotation, _tokenSource.Token)
            );
        }
        catch (OperationCanceledException)
        {
            // Операция отменена - игнорируем
        }
    }

    private async UniTask MovePositionAsync(Vector3 targetPosition, CancellationToken token)
    {
        Vector3 startPosition = _camera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < _durationMoving && !token.IsCancellationRequested)
        {
            float t = elapsedTime / _durationMoving;
            // Используем плавную интерполяцию (можно заменить на любой easing)
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            _camera.transform.position = Vector3.Lerp(startPosition, targetPosition, smoothT);

            elapsedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: token);
        }

        // Гарантируем, что камера окажется в целевой позиции
        if (!token.IsCancellationRequested)
        {
            _camera.transform.position = targetPosition;
        }
    }

    private async UniTask RotateAsync(Quaternion targetRotation, CancellationToken token)
    {
        Quaternion startRotation = _camera.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < _durationRotation && !token.IsCancellationRequested)
        {
            float t = elapsedTime / _durationRotation;
            // Используем плавную интерполяцию
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            _camera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, smoothT);

            elapsedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: token);
        }

        // Гарантируем, что камера окажется в целевом повороте
        if (!token.IsCancellationRequested)
        {
            _camera.transform.rotation = targetRotation;
        }
    }

    [Serializable]
    public struct CameraGamplayPosition
    {
        public Vector3 Direction;
        public Transform Position;
    }
}