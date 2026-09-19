using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanyaBeerExtension;
using TMPro;
using UnityEngine;

public class Platform : MonoBehaviour, IPlatfromBonus
{
    [Header("Gameplay")]
    [field: SerializeField] public Transform Target { get; private set; }
    [field: SerializeField] public Transform SlicePattern { get; private set; }
    [SerializeField] private TextMeshPro _numberText;

    [Header("Animation")]
    [SerializeField] private ParametrBase<Transform> _appearanceAnimation;
    [SerializeField] private ParametrBase<Transform> _destroyAnimation;
    [SerializeField] private MeshRenderer[] _skins;

    [field: Header("Bonus")]
    [field: SerializeField] public bool HasBonus { get; private set; }
    [SerializeField] private GameObject _bonusActivity;
    [SerializeField] private ParametrBase<Transform> _bonusAppearanceAnimation;
    [SerializeField] private ParametrBase<Transform> _bonusDestroyAnimation;

    public Vector3 Direction { get; private set; }

    private MaterialPropertyBlock _block;

    public void SetNumber(int number)
    {
        _numberText.text = number.ToString();
    }

    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    public async UniTask ApplyBonusAsync()
    {
        await _bonusDestroyAnimation.Source
            .DOScale(Vector3.zero, _bonusDestroyAnimation.Duration)
            .SetEase(_bonusDestroyAnimation.Ease)
            .OnComplete(() => _bonusActivity.DisactiveSelf())
            .AsyncWaitForCompletion()
            .AsUniTask();

        await BonusService.Instance.Large();
    }

    public void SetColor(Color color)
    {
        _block ??= new MaterialPropertyBlock();

        foreach (MeshRenderer skin in _skins)
        {
            skin.GetPropertyBlock(_block);
            _block.SetColor("_BaseColor", color);
            _block.SetColor("_Color", color);
            skin.SetPropertyBlock(_block);
        }
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

    public void ActiveBonus()
    {
        HasBonus = true;
    }

    public async UniTask ShowBonusAsync()
    {
        _bonusAppearanceAnimation.Source.localScale = Vector3.zero;
        _bonusActivity.ActiveSelf();

        await _bonusAppearanceAnimation.Source
            .DOScale(Vector3.one, _bonusAppearanceAnimation.Duration)
            .SetEase(_bonusAppearanceAnimation.Ease)
            .AsyncWaitForCompletion()
            .AsUniTask();
    }
}
