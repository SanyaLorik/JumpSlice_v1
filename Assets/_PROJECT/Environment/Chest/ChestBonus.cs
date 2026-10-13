using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanyaBeerExtension;
using UnityEngine;

public class ChestBonus : MonoBehaviour
{
    [SerializeField] private GameObject _mainGameObject;

    [Header("Animation")]
    [Header("Cap")]
    [SerializeField] private ParametrBase<Transform> _cap;
    [SerializeField] private Vector3 _capAngle;
    [SerializeField] private Vector3 _initialAngleCap;

    [Header("Move")]
    [SerializeField] private ParametrBase<Transform> _base;
    [SerializeField] private Vector3 _offsetMoving;

    [Header("Show and Hide")]
    [SerializeField] private ParametrBase<Transform> _showen;
    [SerializeField] private ParametrBase<Transform> _hidden;

    public async UniTask ShowAsync()
    {
        _mainGameObject.transform.localScale = Vector3.zero;
        _mainGameObject.ActiveSelf();

        await _showen.Source
            .DOScale(Vector3.one, _showen.Duration)
            .SetEase(_showen.Ease)
            .AsyncWaitForCompletion()
            .AsUniTask();
    }

    public async UniTask HideAsync()
    {
        UniTask a = _base.Source
                .DOLocalMove(_offsetMoving, _base.Duration)
                .SetEase(_base.Ease)
                .AsyncWaitForCompletion()
                .AsUniTask();

        UniTask b = DOTween.Sequence()
           .Append(_cap.Source
               .DOLocalRotate(_capAngle, _cap.Duration)
               .SetEase(_cap.Ease))
           .Append(_hidden.Source
               .DOScale(Vector3.zero, _hidden.Duration)
               .SetEase(_hidden.Ease))
           .JoinCallback(() =>
            {
                BonusService.Instance.Money();
            })
           .AsyncWaitForCompletion()
           .AsUniTask();

        await UniTask.WhenAll(a, b);
    }
}