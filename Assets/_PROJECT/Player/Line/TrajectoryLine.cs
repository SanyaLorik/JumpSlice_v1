using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanyaBeerExtension;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform _initial;
    [SerializeField] private Transform _final;
    [SerializeField] private int _countPoint;
    
    [field: Header("Movement")]
    [field: SerializeField] public AnimationCurve Trajectory { get; private set; }
    [field: SerializeField] public float Height { get; private set; }

    [Header("Line")]
    [SerializeField] private AnimationCurve _lineEase;
    [SerializeField] private PairedValue<Vector3> _range;
    [SerializeField] private float _durationRange;

    [Header("Fade")]
    [SerializeField] private Ease _easeShown;
    [SerializeField] private float _durationShown;
    [SerializeField] private Ease _easeHide;
    [SerializeField] private float _durationHide;

    public Vector3 Target { get; private set; } = Vector3.zero;

    private void Start()
    {
        MoveAsync().Forget();
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

    private async UniTaskVoid MoveAsync()
    {
        while (destroyCancellationToken.IsCancellationRequested == false)
        {
            Vector3 from = _final.position + _range.From;
            Vector3 to = _final.position + _range.To;

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

            Target = Vector3.Lerp(from, to, lerp);

            Create(Target);

            expendedTime += Time.deltaTime;
            await UniTask.Yield();
        }
        while (expendedTime < _durationRange);
    }

    [ContextMenu("Create")]
    private void CreateInInspector()
    {
        Create(_final.position);
    }

    private void Create(Vector3 target)
    {
        Vector3[] positions = new Vector3[_countPoint + 1];

        for (int i = 0; i <= _countPoint; i++)
        {
            float t = (float)i / (float)_countPoint;
            Vector3 horizontalPosition = Vector3.Lerp(_initial.position, target, t);

            float heightOffset = Trajectory.Evaluate(t) * Height;
            Vector3 verticalOffset = Vector3.up * heightOffset;

            positions[i] = horizontalPosition + verticalOffset;
        }

        _line.SetPositions(positions);
    }
}