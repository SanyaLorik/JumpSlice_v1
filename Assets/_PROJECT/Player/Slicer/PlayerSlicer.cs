using UnityEngine;
using System.Collections;

public class PlayerSlicer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _platform;

    [Header("Settings")]
    [Tooltip("Минимальный размер игрока, чтобы он не исчез полностью (0 = может исчезнуть)")]
    [SerializeField] private float _minScaleFactor = 0.1f;

    [ContextMenu("Test Constrain")]
    public void TestConstrain()
    {
        ConstrainPlayerToPlatform();
    }

    /// <summary>
    /// Мгновенно подгоняет размер и позицию игрока под границы платформы.
    /// Игрок будет "обрезаться" или "сплющиваться" о края, оставаясь внутри.
    /// </summary>
    public void ConstrainPlayerToPlatform()
    {
        if (_player == null || _platform == null)
        {
            Debug.LogWarning("Player or Platform is not assigned!");
            return;
        }

        Bounds playerBounds = GetWorldBounds(_player);
        Bounds platformBounds = GetWorldBounds(_platform);

        // --- Вычисляем пересечение по оси X ---
        float intersectMinX = Mathf.Max(playerBounds.min.x, platformBounds.min.x);
        float intersectMaxX = Mathf.Min(playerBounds.max.x, platformBounds.max.x);
        float targetSizeX = Mathf.Max(_minScaleFactor, intersectMaxX - intersectMinX);
        float targetPosX = (intersectMinX + intersectMaxX) / 2f;

        // --- Вычисляем пересечение по оси Z ---
        float intersectMinZ = Mathf.Max(playerBounds.min.z, platformBounds.min.z);
        float intersectMaxZ = Mathf.Min(playerBounds.max.z, platformBounds.max.z);
        float targetSizeZ = Mathf.Max(_minScaleFactor, intersectMaxZ - intersectMinZ);
        float targetPosZ = (intersectMinZ + intersectMaxZ) / 2f;

        // --- Применяем изменения ---
        // Ось Y не трогаем, чтобы игрок не менял высоту (или можно добавить аналогичную логику при необходимости)
        float targetSizeY = playerBounds.size.y;
        float targetPosY = _player.position.y;

        Vector3 targetWorldSize = new Vector3(targetSizeX, targetSizeY, targetSizeZ);
        Vector3 targetWorldPos = new Vector3(targetPosX, targetPosY, targetPosZ);

        //ApplyWorldTransform(_player, targetWorldPos, targetWorldSize);
        SmoothFitToPlatform();
    }

    /// <summary>
    /// Плавно уменьшает и перемещает игрока в границы платформы.
    /// </summary>
    public void SmoothFitToPlatform(float duration = 2f)
    {
        if (_player == null || _platform == null) return;
        StopAllCoroutines();
        StartCoroutine(SmoothConstrainCoroutine(duration));
    }

    private IEnumerator SmoothConstrainCoroutine(float duration)
    {
        Bounds playerBounds = GetWorldBounds(_player);
        Bounds platformBounds = GetWorldBounds(_platform);

        Vector3 startWorldSize = playerBounds.size;
        Vector3 startPos = _player.position;

        // Вычисляем целевые значения (та же логика пересечения)
        float intersectMinX = Mathf.Max(playerBounds.min.x, platformBounds.min.x);
        float intersectMaxX = Mathf.Min(playerBounds.max.x, platformBounds.max.x);
        float targetSizeX = Mathf.Max(_minScaleFactor, intersectMaxX - intersectMinX);
        float targetPosX = (intersectMinX + intersectMaxX) / 2f;

        float intersectMinZ = Mathf.Max(playerBounds.min.z, platformBounds.min.z);
        float intersectMaxZ = Mathf.Min(playerBounds.max.z, platformBounds.max.z);
        float targetSizeZ = Mathf.Max(_minScaleFactor, intersectMaxZ - intersectMinZ);
        float targetPosZ = (intersectMinZ + intersectMaxZ) / 2f;

        Vector3 targetWorldSize = new Vector3(targetSizeX, startWorldSize.y, targetSizeZ);
        Vector3 targetWorldPos = new Vector3(targetPosX, startPos.y, targetPosZ);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            // Используем SmoothStep для более приятного визуального эффекта "сплющивания"
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector3 currentWorldSize = Vector3.Lerp(startWorldSize, targetWorldSize, smoothT);
            Vector3 currentWorldPos = Vector3.Lerp(startPos, targetWorldPos, smoothT);

            ApplyWorldTransform(_player, currentWorldPos, currentWorldSize);
            yield return null;
        }

        // Финальная фиксация для гарантии точности
        ApplyWorldTransform(_player, targetWorldPos, targetWorldSize);
    }

    // ==========================================
    // Вспомогательные методы
    // ==========================================

    /// <summary>
    /// Получает реальные мировые границы объекта.
    /// Приоритет: Collider -> Renderer -> lossyScale (как запасной вариант).
    /// </summary>
    private Bounds GetWorldBounds(Transform t)
    {
        Collider col = t.GetComponent<Collider>();
        if (col != null) return col.bounds;

        Renderer rend = t.GetComponent<Renderer>();
        if (rend != null) return rend.bounds;

        // Если нет ни коллайдера, ни рендера, используем lossyScale (мировой масштаб)
        // Это работает корректно, только если базовая модель имеет размер 1x1x1
        Vector3 center = t.position;
        Vector3 size = t.lossyScale;
        return new Bounds(center, size);
    }

    /// <summary>
    /// Применяет мировые координаты и размер к локальным параметрам Transform,
    /// корректно учитывая масштаб родительских объектов.
    /// </summary>
    private void ApplyWorldTransform(Transform t, Vector3 worldPos, Vector3 worldSize)
    {
        t.position = worldPos;

        // Чтобы установить мировой размер, нужно разделить его на мировой масштаб родителя
        Vector3 parentLossyScale = t.parent != null ? t.parent.lossyScale : Vector3.one;

        Vector3 newLocalScale = new Vector3(
            worldSize.x / parentLossyScale.x,
            worldSize.y / parentLossyScale.y,
            worldSize.z / parentLossyScale.z
        );

        t.localScale = newLocalScale;
    }
}