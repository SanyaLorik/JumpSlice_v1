using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanyaBeerExtension;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class BackgroundPart : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private PairedValue<float> _speedMove;
    [SerializeField] private PairedValue<float> _speedRotate;
    [SerializeField] private PairedValue<float> _time;

    [Header("Position")]
    [SerializeField] private float _spawnOffset = 100f; // отступ за границей экрана

    [Header("Fade")]
    [SerializeField] private ParametrBase<Image> _fade; // отступ за границей экрана

    private RectTransform _rectTransform;
    private Vector2 _direction;
    private float _currentSpeedMove;
    private float _currentSpeedRotate;
    private float _currentTime;

    private float _intitalAlpha = 0;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _intitalAlpha = _fade.Source.color.a;

        Relocate();
        TickAsync().Forget();
    }

    private async UniTaskVoid TickAsync()
    {
        while (destroyCancellationToken.IsCancellationRequested == false)
        {
            await UniTask.Delay(_currentTime.ToDelayMillisecond(), cancellationToken: destroyCancellationToken);

            await _fade.Source
                .DOFade(0, _fade.Duration)
                .SetEase(_fade.Ease)
                .AsyncWaitForCompletion()
                .AsUniTask();

            Relocate();
        }
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        _rectTransform.anchoredPosition += _currentSpeedMove * Time.deltaTime * _direction;
        _rectTransform.localEulerAngles = _rectTransform.localEulerAngles.AddZ(_currentSpeedRotate * Time.deltaTime);
    }

    private void Relocate()
    {
        // Определяем, с какой стороны экрана появится объект (0 - лево, 1 - право, 2 - низ, 3 - верх)
        int side = Random.Range(0, 4);

        float halfW = Screen.width * 0.5f;
        float halfH = Screen.height * 0.5f;

        // Позиция объекта за пределами экрана (в координатах Canvas)
        switch (side)
        {
            case 0: // слева
                _rectTransform.anchoredPosition = new Vector2(-halfW - _spawnOffset, Random.Range(-halfH, halfH));
                break;
            case 1: // справа
                _rectTransform.anchoredPosition = new Vector2(halfW + _spawnOffset, Random.Range(-halfH, halfH));
                break;
            case 2: // снизу
                _rectTransform.anchoredPosition = new Vector2(Random.Range(-halfW, halfW), -halfH - _spawnOffset);
                break;
            case 3: // сверху
                _rectTransform.anchoredPosition = new Vector2(Random.Range(-halfW, halfW), halfH + _spawnOffset);
                break;
        }

        // Случайная точка внутри экрана, куда полетит объект
        Vector2 targetInsideScreen = new (
            Random.Range(-halfW, halfW),
            Random.Range(-halfH, halfH )
        );

        // Направление движения
        _direction = (targetInsideScreen - _rectTransform.anchoredPosition).normalized;

        // Задание скорости передвижения
        _currentSpeedMove = _speedMove.GetRandom();

        // Задание скорости вращения
        _currentSpeedRotate = _speedRotate.GetRandom();

        // Задание времени
        _currentTime = _time.GetRandom();

        // Востановление прозрачности
        _fade.Source.DOFade(_intitalAlpha, 0.0001f);
    }
}