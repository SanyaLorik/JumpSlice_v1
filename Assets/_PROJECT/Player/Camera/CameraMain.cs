using DG.Tweening;
using UnityEngine;

public class CameraMain : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _duration = 16;
    [SerializeField] private float _strength = 0.32f;
    [SerializeField] private int _vibrato = 0;
    [SerializeField] float _randomness = 0;

    private void Start()
    {
        StartAnimation();
    }

    public void StartAnimation()
    {
        _camera
            .DOShakePosition(_duration, _strength, _vibrato, _randomness)
            .SetLoops(-1, LoopType.Yoyo);
    }
}