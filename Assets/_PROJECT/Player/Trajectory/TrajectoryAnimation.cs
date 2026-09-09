using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanyaBeerExtension;
using UnityEngine;

public class TrajectoryAnimation : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;

    [Header("Debug")]
    [SerializeField] private Transform _final;
    [SerializeField] private int _countPoint;

    [Header("Fade")]
    [SerializeField] private bool _isStartHide = true;
    [SerializeField] private Ease _easeShown;
    [SerializeField] private float _durationShown;
    [SerializeField] private Ease _easeHide;
    [SerializeField] private float _durationHide;

    public Vector3 Target { get; private set; } = Vector3.zero;

    private void Start()
    {
        if (_isStartHide == true)
            _line.DisactiveSelf();
    }

    public async UniTask ShowAnimationAsync()
    {
        Material material = _line.materials[0];

        material.DOFade(0, float.MinValue);

        await material
            .DOFade(1, _durationShown)
            .SetEase(_easeShown)
            .AsyncWaitForCompletion();
    }

    public async UniTask HideAnimationAsync()
    {
        Material material = _line.materials[0];

        material.DOFade(1, float.MinValue);

        await material
            .DOFade(0, _durationHide)
            .SetEase(_easeHide)
            .AsyncWaitForCompletion();
    }

    public void Hide()
    {
        Material material = _line.materials[0];

        material.DOFade(0, float.MinValue);
    }

    public void CreateLine(Vector3 source, Vector3 target, AnimationCurve trajectory, float height)
    {
        Vector3[] positions = new Vector3[_countPoint + 1];

        for (int i = 0; i <= _countPoint; i++)
        {
            float t = (float)i / (float)_countPoint;
            Vector3 horizontalPosition = Vector3.Lerp(source, target, t);

            float heightOffset = trajectory.Evaluate(t) * height;
            Vector3 verticalOffset = Vector3.up * heightOffset;

            positions[i] = horizontalPosition + verticalOffset;
        }

        _line.SetPositions(positions);
    }

    public void CreateLineDebug(Vector3 source, AnimationCurve trajectory, float height)
    {
        Vector3[] positions = new Vector3[_countPoint + 1];

        for (int i = 0; i <= _countPoint; i++)
        {
            float t = (float)i / (float)_countPoint;
            Vector3 horizontalPosition = Vector3.Lerp(source, _final.position, t);

            float heightOffset = trajectory.Evaluate(t) * height;
            Vector3 verticalOffset = Vector3.up * heightOffset;

            positions[i] = horizontalPosition + verticalOffset;
        }

        _line.SetPositions(positions);
    }
}