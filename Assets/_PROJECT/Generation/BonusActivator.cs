using UnityEngine;

public class BonusActivator : MonoBehaviour
{
    [SerializeField] private Transform _player;

    [Header("Настройки размера игрока")]
    [Tooltip("Размер игрока, при котором бонус появляется максимально часто")]
    [SerializeField] private float _minPlayerScale = 0.5f;
    [Tooltip("Размер игрока, при котором бонус появляется редко")]
    [SerializeField] private float _maxPlayerScale = 2f;
    [Tooltip("Минимальный шанс активации (0-1) при большом игроке")]
    [Range(0f, 1f)]
    [SerializeField] private float _minChanceByScale = 0.1f;
    [Tooltip("Максимальный шанс активации (0-1) при маленьком игроке")]
    [Range(0f, 1f)]
    [SerializeField] private float _maxChanceByScale = 1f;

    [Header("Настройки numberCounter")]
    [Tooltip("Значение numberCounter, после которого бонус начинает появляться реже")]
    [SerializeField] private int _counterThreshold = 10;
    [Tooltip("Насколько сильно numberCounter снижает шанс (множитель за каждую единицу сверх порога)")]
    [SerializeField] private float _counterFalloff = 0.05f;
    [Tooltip("Минимальный множитель шанса от numberCounter")]
    [Range(0f, 1f)]
    [SerializeField] private float _minCounterMultiplier = 0.1f;

    /// <summary>
    /// Проверяет, должен ли быть активирован бонус.
    /// </summary>
    public bool IsActive(int numberCounter)
    {
        float chance = CalculateChance(numberCounter);
        return Random.value <= chance;
    }

    /// <summary>
    /// Вычисляет итоговый шанс появления бонуса.
    /// </summary>
    private float CalculateChance(int numberCounter)
    {
        // 1. Шанс в зависимости от размера игрока
        float playerScale = GetPlayerScale();
        // Чем меньше игрок — тем выше шанс
        float scaleT = Mathf.InverseLerp(_minPlayerScale, _maxPlayerScale, playerScale);
        float scaleChance = Mathf.Lerp(_maxChanceByScale, _minChanceByScale, scaleT);

        // 2. Множитель в зависимости от numberCounter
        float counterMultiplier = 1f;
        if (numberCounter > _counterThreshold)
        {
            int excess = numberCounter - _counterThreshold;
            counterMultiplier = Mathf.Max(_minCounterMultiplier, 1f - excess * _counterFalloff);
        }

        return Mathf.Clamp01(scaleChance * counterMultiplier);
    }

    /// <summary>
    /// Возвращает усреднённый размер игрока (по осям X/Y/Z).
    /// </summary>
    private float GetPlayerScale()
    {
        if (_player == null)
            return _maxPlayerScale;

        Vector3 s = _player.localScale;
        return (s.x + s.z) / 2f;
    }
}