using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanyaBeerExtension;
using TMPro;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatfromBonus
{
    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public Transform SlicePattern { get; private set; }
    [SerializeField] private TextMeshPro _numberText;

    [field: SerializeField] public bool HasBonus { get; private set; }

    [SerializeField] private ParametrBase<Transform> _appearanceAnimation;
    [SerializeField] private ParametrBase<Transform> _destroyAnimation;

    public Vector3 Direction { get; private set; }

    public void SetNumber(int number)
    {
        _numberText.text = number.ToString();
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public async UniTask ApplyAsync()
    {
        await BonusService.Instance.Large();
    }

    public async UniTask AppearanceAnimationAsync()
    {
        _appearanceAnimation.Source.localScale = Vector2.zero;

        await _appearanceAnimation.Source
            .DOScale(Vector3.one, _appearanceAnimation.Duration)
            .SetEase(_appearanceAnimation.Ease)
            .AsyncWaitForCompletion()
            .AsUniTask();
    }

    public void ZeroScale()
    {
        _appearanceAnimation.Source.localScale = Vector3.zero;
    }

    public async UniTask DestroyAnimationAsync()
    {
        await _destroyAnimation.Source
            .DOScale(Vector3.zero, _destroyAnimation.Duration)
            .SetEase(_destroyAnimation.Ease)
            .AsyncWaitForCompletion()
            .AsUniTask();

        DestoryNoAnimation();
    }

    public void DestoryNoAnimation()
    {
        Destroy(gameObject);
    }
}
