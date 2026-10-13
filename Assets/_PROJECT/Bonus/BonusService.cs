using Cysharp.Threading.Tasks;
using UnityEngine;

public class BonusService : MonoBehaviour
{
    [SerializeField] private BonusLarge _bonusLarge;
    [SerializeField] private BonusMoney _bonusMoney;

    public static BonusService Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public async UniTask LargeAsync()
    {
        AudioPlayerManager.Instance.Play(AudioClipType.LargeBonus);

        await _bonusLarge.Large();
    }

    public FlyingMoney Money()
    {
        return _bonusMoney.Money();
    }
}
