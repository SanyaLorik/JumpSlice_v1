using Cysharp.Threading.Tasks;
using SanyaBeerExtension;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromGenerator : MonoBehaviour
{
    [field: SerializeField] public Platform InitalPlatform { get; private set; }

    [Header("Parameters")]
    [SerializeField] private PositionSpawner<Platform> _container;
    [SerializeField] private Platform[] _prefabs;
    [SerializeField] private Vector3[] _direction;
    [SerializeField] private Vector3 _initialDirection;
    [SerializeField] private PairedValue<float> _range;
    [SerializeField] private int _countAnimationDestroy;

    [Header("Anti-repeat")]
    [SerializeField, Min(1)] private int _maxRepeatCount = 2;

    private const int _initialNumberCount = 1;

    private Vector3 _position;
    public int NumberCounter { get; private set; } = _initialNumberCount;

    private List<Platform> _platforms = new(16);

    // История последних выбранных префабов
    private Platform _lastPrefab;
    private int _lastPrefabRepeat;

    // То же самое для направлений (опционально)
    private Vector3 _lastDirection;
    private int _lastDirectionRepeat;

    public Platform Generate()
    {
        Vector3 direction = CalculateNewPositionAndReturnDirection();

        Platform platform = Spawn();
        platform.SetNumber(NumberCounter);
        platform.SetDirection(direction);
        platform.ZeroScale();

        _platforms.Add(platform);

        NumberCounter++;

        return platform;
    }

    public async UniTask DestroyAllAsync()
    {
        ResetPosition();

        if (_platforms.Count == 0)
        {
            await UniTask.CompletedTask;
            return;
        }

        int count = _platforms.Count;
        int animatedCount = Mathf.Min(_countAnimationDestroy, count);

        for (int i = count - 1; i >= 0; i--)
        {
            if (i >= count - animatedCount)
                await _platforms[i].DestroyAnimationAsync();
            else
                _platforms[i].DestoryNoAnimation();
        }

        _platforms.Clear();

        NumberCounter = _initialNumberCount;

        ResetRepeatState();
    }

    private Vector3 CalculateNewPositionAndReturnDirection()
    {
        Vector3 direction = GetNonRepeatingDirection();
        if (NumberCounter == 1)
            direction = _initialDirection;

        float distance = UnityEngine.Random.Range(_range.From, _range.To);
        Vector3 offset = direction * distance;

        _position += offset;

        return direction;
    }

    private Platform Spawn()
    {
        Platform prefab = GetNonRepeatingPrefab();

        Platform platform = _container.Spawn(prefab, _position);
        platform.name = $"{prefab.name}_{NumberCounter}";

        return platform;
    }

    private Platform GetNonRepeatingPrefab()
    {
        if (_prefabs == null || _prefabs.Length == 0)
            return null;

        // Если префабов мало — не ограничиваем (иначе зациклимся)
        int allowedRepeat = Mathf.Min(_maxRepeatCount, _prefabs.Length);

        for (int attempt = 0; attempt < 32; attempt++)
        {
            Platform candidate = _prefabs.GetRandomElement();

            bool sameAsLast = candidate == _lastPrefab;
            if (sameAsLast && _lastPrefabRepeat >= allowedRepeat)
                continue;

            // Обновляем счётчики
            if (sameAsLast)
                _lastPrefabRepeat++;
            else
            {
                _lastPrefab = candidate;
                _lastPrefabRepeat = 1;
            }

            return candidate;
        }

        // Фолбэк: если не удалось найти другой префаб — сбрасываем счётчик
        _lastPrefabRepeat = 0;
        return _lastPrefab != null ? _lastPrefab : _prefabs[0];
    }

    private Vector3 GetNonRepeatingDirection()
    {
        if (_direction == null || _direction.Length == 0)
            return Vector3.forward;

        int allowedRepeat = Mathf.Min(_maxRepeatCount, _direction.Length);

        for (int attempt = 0; attempt < 32; attempt++)
        {
            Vector3 candidate = _direction.GetRandomElement();

            bool sameAsLast = candidate == _lastDirection;
            if (sameAsLast && _lastDirectionRepeat >= allowedRepeat)
                continue;

            if (sameAsLast)
                _lastDirectionRepeat++;
            else
            {
                _lastDirection = candidate;
                _lastDirectionRepeat = 1;
            }

            return candidate;
        }

        _lastDirectionRepeat = 0;
        return _lastDirection != Vector3.zero ? _lastDirection : _direction[0];
    }

    private void ResetRepeatState()
    {
        _lastPrefab = null;
        _lastPrefabRepeat = 0;
        _lastDirection = Vector3.zero;
        _lastDirectionRepeat = 0;
    }

    private void ResetPosition()
    {
        _position = InitalPlatform.transform.position;
    }
}