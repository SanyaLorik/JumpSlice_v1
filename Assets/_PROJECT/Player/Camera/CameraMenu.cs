using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    [SerializeField] private Transform _initialPoint;
    [SerializeField] private Camera _camera;

    [Header("ReturnToInitial")]
    [SerializeField] private float _returnPositionDuration = 1;
    [SerializeField] private Ease _returnPositionEase = Ease.Unset;
    [SerializeField] private float _returnRotationDuration = 1;
    [SerializeField] private Ease _returnRotationEase = Ease.Unset;

    [Header("Calmness")]
    [SerializeField] private float _calmDuration = 16;
    [SerializeField] private float _calmStrength = 0.32f;
    [SerializeField] private int _calmVibrato = 0;
    [SerializeField] float _calmRandomness = 0;

    private Tween _tween;

    public async UniTask ReturnCamera()
    {
        UniTask position = _camera.transform
            .DOMove(_initialPoint.position, _returnPositionDuration)
            .SetEase(_returnPositionEase)
            .AsyncWaitForCompletion()
            .AsUniTask();

        UniTask rotation = _camera.transform
            .DORotateQuaternion(_initialPoint.rotation, _returnRotationDuration)
            .SetEase(_returnRotationEase)
            .AsyncWaitForCompletion()
            .AsUniTask();

        await UniTask
            .WhenAll(position, rotation)
            .AttachExternalCancellation(destroyCancellationToken);
    } 

    public void StartAnimation()
    {
        _tween?.Kill();

        _tween = _camera
            .DOShakePosition(_calmDuration, _calmStrength, _calmVibrato, _calmRandomness)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopAnimation()
    {
        _tween?.Kill();
    }
}
