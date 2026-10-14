using Cysharp.Threading.Tasks;
using SanyaBeerExtension;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{
    [SerializeField] private float _duration = 1;

    [Header("Moving")]
    [SerializeField] private AnimationCurve _movingVertical = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _movinghorizantal = AnimationCurve.Linear(0, 0, 1, 1);

    public async UniTask MoveAsync(Transform player, Vector3 target, AnimationCurve trajectory, float height, Vector3 direction)
    {
        float expendedTime = 0;

        Vector3 initial = transform.position;
        Vector3 final = target;

        Quaternion startRot = player.rotation;
        Vector3 axis = direction == Vector3.forward ? player.right : player.forward; // или Vector3.right, если ось мировая

        do
        {
            // Нормализованное время (0..1)
            float t = Mathf.Clamp01(expendedTime / _duration);

            Move(player, trajectory, height, initial, final, t);

            player.rotation = Quaternion.AngleAxis(360f * t, axis) * startRot;

            expendedTime += Time.deltaTime;
            await UniTask.Yield();
        }
        while (expendedTime < _duration);
    }

    private void Move(Transform player, AnimationCurve trajectory, float height, Vector3 initial, Vector3 final, float t)
    {
        // Горизонтальное движение по кривой
        float horizontalLerp = _movinghorizantal.Evaluate(t);
        Vector3 horizontalPosition = Vector3.Lerp(initial, final, horizontalLerp);

        // Вертикальное движение по кривой
        float verticalLerp = _movingVertical.Evaluate(t);
        float heightOffset = trajectory.Evaluate(verticalLerp) * height;
        Vector3 verticalOffset = Vector3.up * heightOffset;

        // Финальная позиция
        player.position = horizontalPosition + verticalOffset;
    }
}