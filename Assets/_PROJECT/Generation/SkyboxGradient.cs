using Cysharp.Threading.Tasks;
using SanyaBeerExtension;
using System.Threading;
using UnityEngine;

public class SkyboxGradient : MonoBehaviour
{
    [SerializeField] private Gradient[] _gradients;
    [SerializeField] private PairedValue<int> _rangeLenghtGradientNumber;

    [Header("Smooth")]
    [SerializeField] private float _smoothDuration = 0.5f;

    private Gradient _currentGradient;
    private int _gradientNumberCounter = 0;
    private int _gradientLenght = 0;

    private Color _currentColor;
    private Color _targetColor;
    private Color _initialColor; // запоминаем цвет на старте

    private Material _skyboxMaterial;
    private CancellationTokenSource _cts;

    private static readonly int TintId = Shader.PropertyToID("_Tint");

    private void Start()
    {
        _skyboxMaterial = new Material(RenderSettings.skybox);
        RenderSettings.skybox = _skyboxMaterial;

        ResetGradient();

        _currentColor = _targetColor = _currentGradient.Evaluate(0f);
        _initialColor = _currentColor; // <-- запомнили исходный
        ApplyColor(_currentColor);
    }

    public void UpdateColor(int numberCounter)
    {
        if (_gradientLenght <= _gradientNumberCounter)
            ResetGradient();

        _gradientNumberCounter++;
        float ratio = (float)_gradientNumberCounter / (float)_gradientLenght;

        _targetColor = _currentGradient.Evaluate(ratio);
        SmoothToTargetAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    /// <summary>
    /// Плавно возвращает скайбокс к цвету, который был в Start().
    /// </summary>
    public void ResetToInitialColor()
    {
        SmoothToColorAsync(_initialColor, this.GetCancellationTokenOnDestroy());
    }

    /// <summary>
    /// Плавно возвращает скайбокс к первому цвету текущего градиента
    /// и сбрасывает счётчики, чтобы прогресс начался заново.
    /// </summary>
    public void ResetToGradientStart()
    {
        _currentGradient = _gradients.GetRandomElement();
        _gradientLenght = UnityEngine.Random.Range(_rangeLenghtGradientNumber.From, _rangeLenghtGradientNumber.To);
        _gradientNumberCounter = 0;

        SmoothToColorAsync(_currentGradient.Evaluate(0f), this.GetCancellationTokenOnDestroy());
    }

    private void SmoothToColorAsync(Color end, CancellationToken token)
    {
        _targetColor = end;
        SmoothToTargetAsync(token).Forget();
    }

    private async UniTaskVoid SmoothToTargetAsync(CancellationToken token)
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = CancellationTokenSource.CreateLinkedTokenSource(token);

        Color start = _currentColor;
        Color end = _targetColor;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, _smoothDuration);
            _currentColor = Color.Lerp(start, end, Mathf.Clamp01(t));
            ApplyColor(_currentColor);

            await UniTask.Yield(PlayerLoopTiming.Update, _cts.Token);
        }

        _currentColor = end;
        ApplyColor(_currentColor);
    }

    private void ApplyColor(Color color)
    {
        if (_skyboxMaterial == null) return;
        _skyboxMaterial.SetColor(TintId, color);
    }

    private void ResetGradient()
    {
        _currentGradient = _gradients.GetRandomElement();
        _gradientLenght = UnityEngine.Random.Range(_rangeLenghtGradientNumber.From, _rangeLenghtGradientNumber.To);
        _gradientNumberCounter = 0;
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();

        if (_skyboxMaterial != null)
            Destroy(_skyboxMaterial);
    }
}