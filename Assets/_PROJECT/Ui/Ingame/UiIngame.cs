using DG.Tweening;
using SanyaBeerExtension;
using TMPro;
using UnityEngine;

public class UiIngame : MonoBehaviour
{
    [Header("Money")]
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private PositionSpawnerWithPrefab<FlyingMoney> _moneySpawner;
    [SerializeField] private PairedValue<float> _moneySpawnRange;
    [SerializeField] private Transform _moneySpawnpoint;
    [SerializeField] private Transform _moneyTarget;
    [SerializeField] private ParametrBase _moneyMoveParametr;
    [SerializeField] private ParametrBase _moneyFadeParametr;

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
        Vector3 offset = new()
        {
            x = _moneySpawnRange.GetRandom(),
            y = _moneySpawnRange.GetRandom()
        };

        Vector3 position = _moneySpawnpoint.position + offset;
        FlyingMoney flyingMoney = _moneySpawner.Spawn(position);
        flyingMoney.SetText(money);

        DOTween.Sequence()
            .Append(flyingMoney.transform
                .DOMove(_moneyTarget.position, _moneyMoveParametr.Duration)
                .SetEase(_moneyMoveParametr.Ease))
            .Append(flyingMoney.CanvasGroup
                .DOFade(0, _moneyFadeParametr.Duration)
                .SetEase(_moneyFadeParametr.Ease))
            .OnComplete(() =>
            {
                Destroy(flyingMoney.gameObject);
            });
    }
}