using DG.Tweening;
using SanyaBeerExtension;
using System;
using UnityEngine;

[Serializable]
public class BonusMoney
{
    [SerializeField] private PositionSpawnerWithPrefab<FlyingMoney> _moneySpawner;
    [SerializeField] private PairedValue<float> _moneySpawnRange;
    [SerializeField] private Transform _moneySpawnpoint;
    [SerializeField] private Transform _moneyTarget;
    [SerializeField] private ParametrBase _moneyMoveParametr;
    [SerializeField] private ParametrBase _moneyFadeParametr;

    public FlyingMoney Money(int money = 123)
    {
        Vector3 offset = new()
        {
            x = _moneySpawnRange.GetRandom(),
            y = _moneySpawnRange.GetRandom()
        };

        Vector3 position = _moneySpawnpoint.position + offset;
        FlyingMoney flyingMoney = _moneySpawner.Spawn(position);

        DOTween.Sequence()
            .Append(flyingMoney.transform
                .DOMove(_moneyTarget.position, _moneyMoveParametr.Duration)
                .SetEase(_moneyMoveParametr.Ease))
            .Append(flyingMoney.CanvasGroup
                .DOFade(0, _moneyFadeParametr.Duration)
                .SetEase(_moneyFadeParametr.Ease))
            .OnComplete(() =>
            {
                UnityEngine.Object.Destroy(flyingMoney.gameObject);
            });

        flyingMoney.SetText(money);

        return flyingMoney;
    }
}
