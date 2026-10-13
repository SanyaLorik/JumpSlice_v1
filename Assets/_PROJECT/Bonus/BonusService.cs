using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BonusService : MonoBehaviour
{
    public static BonusService Instance { get; private set; }

    [SerializeField] private BonusLarge _bonusLarge;
    [SerializeField] private BonusMoney _bonusMoney;

    [Inject] private GameData _gameData;
    [Inject] private IGameSave _gameSave;

    private void Awake()
    {
        Instance = this;
    }

    public async UniTask LargeAsync()
    {
        AudioPlayerManager.Instance.Play(AudioClipType.LargeBonus);

        await _bonusLarge.Large();
    }

    public FlyingMoney Money(int money = -1)
    {
        if (money == -1)
        {
            FlyingMoney flyingMoney = _bonusMoney.Money();
            int countMoney = _gameData.GetChestMoney(_gameSave.GetSave<GameSave>().ProfitLevel);
            flyingMoney.SetText(countMoney);

            return flyingMoney;
        }

        return _bonusMoney.Money(money);
    }
}
