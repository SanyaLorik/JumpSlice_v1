using Cysharp.Threading.Tasks;
using UnityEngine;

public class BonusService : MonoBehaviour
{
    [SerializeField] private BonusLarge _bonusLarge;

    public static BonusService Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public async UniTask Large()
    {
        await _bonusLarge.Large();
    }
}
