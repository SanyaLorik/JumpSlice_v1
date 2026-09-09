using Cysharp.Threading.Tasks;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private AnimationCurve _verticalCurve;
    [SerializeField] private AnimationCurve _horizantalCurve;
    [SerializeField] private float _duration;

    public async UniTask MoveAsync(Transform player, Vector3 target, AnimationCurve trajectory, float height)
    {
        float expendedTime = 0;

        Vector3 initial = transform.position;
        Vector3 final = target;

        do
        {
            // Нормализованное время (0..1)
            float t = Mathf.Clamp01(expendedTime / _duration);

            // Горизонтальное движение по кривой
            float horizontalLerp = _horizantalCurve.Evaluate(t);
            Vector3 horizontalPosition = Vector3.Lerp(initial, final, horizontalLerp);

            // Вертикальное движение по кривой
            float verticalLerp = _verticalCurve.Evaluate(t);
            float heightOffset = trajectory.Evaluate(verticalLerp) * height;
            Vector3 verticalOffset = Vector3.up * heightOffset;

            // Финальная позиция
            player.position = horizontalPosition + verticalOffset;

            expendedTime += Time.deltaTime;
            await UniTask.Yield();
        }
        while (expendedTime < _duration);
    }
}