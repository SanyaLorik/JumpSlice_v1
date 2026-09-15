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
    public int NumberCounter { get; private set; } = 1;

    private void Start()
    {
        _position = _initalPlatform.transform.position;
    }

    public Platform Generate()
    {
        Vector3 direction = CalculateNewPositionAndReturnDirection();

        Platform platform = Spawn();
        platform.SetNumber(NumberCounter);
        platform.SetDirection(direction);

        NumberCounter++;

        return platform;
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

    private Platform Spawn()
    {
        Platform prefab = _prefabs.GetRandomElement();

        Platform platform = _container.Spawn(prefab, _position);
        platform.name = $"{prefab.name}_{NumberCounter}";

        return platform;
    }
}
