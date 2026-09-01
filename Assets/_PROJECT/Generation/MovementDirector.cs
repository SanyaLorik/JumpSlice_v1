using UnityEngine;

public class MovementDirector : MonoBehaviour
{
    [SerializeField] private PlatfromGenerator _generator;
    [SerializeField] private Movement _movement;

    private void OnEnable()
    {
        _movement.OnMoved += OnNext;
    }

    private void OnDisable()
    {
        _movement.OnMoved -= OnNext;
    }

    public void StartDirection()
    {
        Next();
    }

    private void OnNext()
    {
        Next();
    }

    private void Next()
    {
        Platform platform = _generator.Generate();
        _movement.Target = platform.Target;
    }
}