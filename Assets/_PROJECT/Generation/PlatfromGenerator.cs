using Cysharp.Threading.Tasks;
using NUnit.Framework;
using SanyaBeerExtension;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromGenerator : MonoBehaviour
{
    [SerializeField] private Platform _initalPlatform;

    [Header("Parameters")]
    [SerializeField] private PositionSpawner<Platform> _container;
    [SerializeField] private Platform[] _prefabs;
    [SerializeField] private Vector3[] _direction;
    [SerializeField] private Vector3 _initialDirection;
    [SerializeField] private PairedValue<float> _range;
    [SerializeField] private int _countAnimationDestroy;

    private const int _initialNumberCount = 1;

    private Vector3 _position;
    public int NumberCounter { get; private set; } = _initialNumberCount;

    private List<Platform> _platforms = new(16);

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

        for (int i = 0; i < _platforms.Count; i++)
        {
            if (i < _countAnimationDestroy)
                await _platforms[i].DestroyAnimationAsync();
            else
                _platforms[i].DestoryNoAnimation();
        }

        _platforms.Clear();

        NumberCounter = _initialNumberCount;
    }

    private Vector3 CalculateNewPositionAndReturnDirection()
    {
        Vector3 direction = _direction.GetRandomElement();
        if (NumberCounter == 1)
            direction = _initialDirection;

        float distance = UnityEngine.Random.Range(_range.From, _range.To);
        Vector3 offset = direction * distance;

        _position += offset;

        return direction;
    }

    private void ResetPosition()
    {
        _position = _initalPlatform.transform.position;
    }

    private Platform Spawn()
    {
        Platform prefab = _prefabs.GetRandomElement();

        Platform platform = _container.Spawn(prefab, _position);
        platform.name = $"{prefab.name}_{NumberCounter}";

        return platform;
    }
}
