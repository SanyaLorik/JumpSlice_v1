using Architecture_M;
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

    private void Awake()
    {
        Hide();
    }

    public void OpenAnimation()
    {
        DOTween.Sequence()
            .Append(_base.Source
                .DOLocalMove(_offsetMoving, _base.Duration)
                .SetEase(_base.Ease))
            .Append(_cap.Source
                .DOLocalRotate(_capAngle, _cap.Duration)
                .SetEase(_cap.Ease))
            .OnComplete(() =>
            {
                HideAnimation();
                BonusService.Instance.Money();
            });
    }

    public void SetPosition(Vector3 position)
    {
        _mainGameObject.transform.position = position;
    }

    public void ShowAnimation()
    {
        _mainGameObject.transform.localScale = Vector3.zero;
        _mainGameObject.ActiveSelf();

        _showen.Source
            .DOScale(Vector3.one, _showen.Duration)
            .SetEase(_showen.Ease);
    }

    private void HideAnimation()
    {
        _mainGameObject.transform.localScale = Vector3.one;
        _mainGameObject.ActiveSelf();

        _hidden.Source
            .DOScale(Vector3.zero, _hidden.Duration)
            .SetEase(_hidden.Ease)
            .OnComplete(() =>
            {
                Hide();
                ReturnToInitialState();
            });
    }

    private void Hide()
    {
        _mainGameObject.DisactiveSelf();
    }

    private void ReturnToInitialState()
    {
        _cap.Source.eulerAngles = _initialAngleCap;
        _base.Source.localPosition -= _offsetMoving;
    }
}