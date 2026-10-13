using DG.Tweening;
using SanyaBeerExtension;
using TMPro;
using UnityEngine;

public class UiIngame : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] private TextMeshProUGUI _moneyText;

    [Header("Record")]
    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private ParametrBase<Transform> _recordUpAnimation;
    [SerializeField] private ParametrBase<Transform> _recordDownAnimation;
    [SerializeField] private Vector3 _recordUpScale;
    [SerializeField] private float _recordDelay;

    public void SetRecord(int record)
    {
        _recordText.text = record.ToString();
    }

    public void SetMoney(int money)
    {
        _moneyText.text = money.ToString();
    }

    public void AnimateRecord()
    {
        DOTween.Sequence()
            .Append(_recordUpAnimation.Source
                .DOScale(_recordUpScale, _recordUpAnimation.Duration)
                .SetEase(_recordUpAnimation.Ease))
            .AppendInterval(_recordDelay)
            .Append(_recordDownAnimation.Source
                 .DOScale(Vector3.one, _recordDownAnimation.Duration)
                 .SetEase(_recordDownAnimation.Ease));
    }

    public void AnimateMoney(int money)
    {
        BonusService.Instance.Money(money);
    }
}