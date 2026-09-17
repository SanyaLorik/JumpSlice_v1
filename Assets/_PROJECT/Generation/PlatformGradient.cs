using SanyaBeerExtension;
using UnityEngine;

public class PlatformGradient : MonoBehaviour
{
    [SerializeField] private Gradient[] _gradients;
    [SerializeField] private PairedValue<int> _rangeLenghtGradientNumber;

    private Gradient _currentGradient;
    private int _gradientNubmerCounter = 0;
    private int _gradientLenght = 0;

    private void Start()
    {
        ResetGradient();
    }

    public Color GetColor(int numberCounter)
    {
        if (_gradientLenght <= _gradientNubmerCounter)
            ResetGradient();

        _gradientNubmerCounter++;
        float ratio = (float)_gradientNubmerCounter / (float)_gradientLenght;

        return _currentGradient.Evaluate(ratio);
    }

    private void ResetGradient()
    {
        _currentGradient = _gradients.GetRandomElement();
        _gradientLenght = UnityEngine.Random.Range(_rangeLenghtGradientNumber.From, _rangeLenghtGradientNumber.To);
        _gradientNubmerCounter = 0;
    }
}