using SanyaBeerExtension;
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

    private Vector3 _position;
    private int _numberCounter = 1;

    private void Start()
    {
        _position = _initalPlatform.transform.position;
    }

    public Platform Generate()
    {
        Vector3 direction = CalculateNewPositionAndReturnDirection();

        Platform platform = Spawn();
        platform.SetNumber(_numberCounter);
        platform.SetDirection(direction);

        _numberCounter++;

        return platform;
    }

    private Vector3 CalculateNewPositionAndReturnDirection()
    {
        Vector3 direction = _direction.GetRandomElement();
        if (_numberCounter == 1)
            direction = _initialDirection;

        float distance = UnityEngine.Random.Range(_range.From, _range.To);
        Vector3 offset = direction * distance;

        _position += offset;

        return direction;
    }

    private Platform Spawn()
    {
        Platform prefab = _prefabs.GetRandomElement();

        Platform platform = _container.Spawn(prefab, _position);
        platform.name = $"{prefab.name}_{_numberCounter}";

        return platform;
    }
}
