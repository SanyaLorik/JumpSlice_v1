using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class TrajectoryAnimation : MonoBehaviour
{
    [SerializeField] private Transform _center;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Vector3 _offset = new(0, 1, 0);

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

    private Material _material;
    private Tween _tween;

    private void Start()
    {
        _material = _line.materials[0];

        if (_isStartHide == true)
            Hide();
    }

    public void ShowAnimation()
    {
        _tween?.Kill();

        _tween = _material.DOFade(0, float.MinValue);

        _tween = _material
            .DOFade(1, _durationShown)
            .SetEase(_easeShown);
    }

    public void HideAnimation()
    {
        _tween?.Kill();

        _tween = _material.DOFade(1, float.MinValue);

        _tween = _material
            .DOFade(0, _durationHide)
            .SetEase(_easeHide);
    }

    public void Hide()
    {
        Material material = _line.materials[0];

        material.DOFade(0, float.MinValue);
    }

    public void CreateLine(Vector3 target, AnimationCurve trajectory, float height)
    {
        Vector3[] positions = new Vector3[_countPoint + 1];

        for (int i = 0; i <= _countPoint; i++)
        {
            float t = (float)i / (float)_countPoint;
            Vector3 horizontalPosition = Vector3.Lerp(_center.position, target, t);

            float heightOffset = trajectory.Evaluate(t) * height;
            Vector3 verticalOffset = Vector3.up * heightOffset;

            positions[i] = horizontalPosition + verticalOffset + _offset;
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