using Cysharp.Threading.Tasks;
using EzySlice;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerSlicer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private Material _material; 
    [SerializeField] private float _duration;

    [Header("Settings")]
    [Tooltip("Минимальный размер игрока, чтобы он не исчез полностью (0 = может исчезнуть)")]
    [SerializeField] private float _minScaleFactor = 0.1f;

    [Header("Debug")]
    [SerializeField] private Transform _platformDebug;

    private CancellationTokenSource _tokenSource;

    [ContextMenu("Test Constrain")]
    public void TestConstrain()
    {
        ConstrainPlayerToPlatform(_platformDebug);
    }

    /// <summary>
    /// Мгновенно подгоняет размер и позицию игрока под границы платформы.
    /// Игрок будет "обрезаться" или "сплющиваться" о края, оставаясь внутри.
    /// </summary>
    public void ConstrainPlayerToPlatform(Transform platform)
    {
        if (_player == null || platform == null)
        {
            Debug.LogWarning("Player or Platform is not assigned!");
            return;
        }

        Bounds playerBounds = GetWorldBounds(_player);
        Bounds platformBounds = GetWorldBounds(platform);

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

        ApplyWorldTransform(_player, targetWorldPos, targetWorldSize);
        //SmoothFitToPlatformAsync(platform).Forget();
    }

    /// <summary>
    /// Плавно уменьшает и перемещает игрока в границы платформы.
    /// </summary>
    public async UniTask SmoothFitToPlatformAsync(Transform platform)
    {
        if (_player == null || platform == null)
            return;

        if (_tokenSource != null)
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();

            _tokenSource = null;
        }

        _tokenSource = new();

        Cross(platform);
        await SmoothConstrainAsync(platform);
    }

    private async UniTask SmoothConstrainAsync(Transform platform)
    {
        Bounds playerBounds = GetWorldBounds(_player);
        Bounds platformBounds = GetWorldBounds(platform);

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
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _duration);

            // Используем SmoothStep для более приятного визуального эффекта "сплющивания"
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            Vector3 currentWorldSize = Vector3.Lerp(startWorldSize, targetWorldSize, smoothT);
            Vector3 currentWorldPos = Vector3.Lerp(startPos, targetWorldPos, smoothT);

            ApplyWorldTransform(_player, currentWorldPos, currentWorldSize);
            await UniTask.Yield(cancellationToken: _tokenSource.Token);
        }

        // Финальная фиксация для гарантии точности
        ApplyWorldTransform(_player, targetWorldPos, targetWorldSize);
    }

    private void Cross(Transform platform)
    {
        if (_player == null || platform == null)
        {
            Debug.LogWarning("Player or Platform is not assigned!");
            return;
        }

        // Получаем размеры объектов с учетом scale (только X и Z)
        Vector3 playerSize = _player.localScale;
        Vector3 platformSize = platform.localScale;

        // Получаем позиции объектов
        Vector3 playerPos = _player.position;
        Vector3 platformPos = platform.position;

        // Вычисляем границы платформы по X и Z
        float platformLeft = platformPos.x - platformSize.x / 2f;
        float platformRight = platformPos.x + platformSize.x / 2f;
        float platformBack = platformPos.z - platformSize.z / 2f;
        float platformForward = platformPos.z + platformSize.z / 2f;

        // Вычисляем границы игрока по X и Z
        float playerLeft = playerPos.x - playerSize.x / 2f;
        float playerRight = playerPos.x + playerSize.x / 2f;
        float playerBack = playerPos.z - playerSize.z / 2f;
        float playerForward = playerPos.z + playerSize.z / 2f;

        // Список для точек на границе платформы
        List<Vector3> edgePoints = new List<Vector3>();
        List<Vector3> directionPoints = new List<Vector3>();

        // Проверка по оси X
        if (playerLeft < platformLeft)
        {
            // Точка на левой границе платформы
            edgePoints.Add(new Vector3(platformLeft, playerPos.y, playerPos.z));
            directionPoints.Add(Vector3.left);
        }
        if (playerRight > platformRight)
        {
            // Точка на правой границе платформы
            edgePoints.Add(new Vector3(platformRight, playerPos.y, playerPos.z));
            directionPoints.Add(Vector3.right);
        }

        // Проверка по оси Z
        if (playerBack < platformBack)
        {
            // Точка на задней границе платформы
            edgePoints.Add(new Vector3(playerPos.x, playerPos.y, platformBack));
            directionPoints.Add(Vector3.back);
        }
        if (playerForward > platformForward)
        {
            // Точка на передней границе платформы
            edgePoints.Add(new Vector3(playerPos.x, playerPos.y, platformForward));
            directionPoints.Add(Vector3.forward);
        }

        int lenght = Mathf.Min(edgePoints.Count, directionPoints.Count);
         if (lenght < 4)
        {
            SetupSliceHull(edgePoints, directionPoints);
        }
        else
        {
            Debug.Log("Whole parts of player are killed!");
        }

        // Преобразуем список в массив
        //Vector3[] resultPoints = edgePoints.ToArray();

        /*
        // Выводим результат
        if (resultPoints.Length > 0)
        {
            Debug.Log($"Player is OUTSIDE the platform boundaries!");
            Debug.Log($"Platform X: [{platformLeft:F2}, {platformRight:F2}], Z: [{platformBack:F2}, {platformForward:F2}]");
            Debug.Log($"Player X: [{playerLeft:F2}, {playerRight:F2}], Z: [{playerBack:F2}, {playerForward:F2}]");

            // Выводим все точки
            for (int i = 0; i < resultPoints.Length; i++)
                Debug.Log($"Edge point {i + 1}: ({resultPoints[i].x:F2}, {resultPoints[i].y:F2}, {resultPoints[i].z:F2})");
        }
        else
        {
            Debug.Log("Player is INSIDE the platform boundaries.");
        }
        */
    }

    private void SetupSliceHull(IReadOnlyList<Vector3> points, IReadOnlyList<Vector3> directions)
    {
        int lenght = Mathf.Min(points.Count, directions.Count);
        for (int i = 0; i < lenght; i++)
        {
            SlicedHull slicedHull = _player.gameObject.Slice(points[i], directions[i]);

            GameObject lower = slicedHull.CreateLowerHull(_player.gameObject, _material);
            GameObject upper = slicedHull.CreateUpperHull(_player.gameObject, _material);

            float lowerDistance = (_player.position - lower.transform.position).sqrMagnitude;
            float upperDistance = (_player.position - upper.transform.position).sqrMagnitude;

            if (upperDistance > lowerDistance)
            {
                lower.AddComponent<Rigidbody>();
                Destroy(upper);
            }
            else
            {
                upper.AddComponent<Rigidbody>();
                Destroy(lower);
            }
        }
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
        if (col != null) 
            return col.bounds;

        Renderer rend = t.GetComponent<Renderer>();
        if (rend != null)
            return rend.bounds;

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