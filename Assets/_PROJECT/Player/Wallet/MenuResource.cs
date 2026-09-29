using Architecture_M;
using UnityEngine;
using Zenject;

public class MenuResource : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField] private UiMenu _uiIngame;

    [Header("Stats")]
    [SerializeField] private Wallet _money;
    [SerializeField] private Wallet _record;

    private GameSave _save;

    [Inject]
    private void Construct(IGameSave gameSave)
    {
        _save = gameSave.GetSave<GameSave>();
    }

    private void Awake()
    {
        _money.Add(_save.Money);
    }

    private void OnEnable()
    {
        _money.OnChanged += OnAddMoney;
    }

    private void OnDisable()
    {
        _money.OnChanged -= OnAddMoney;
    }

    public void OnAddMoney(int value)
    {
        _uiIngame.SetMoney(value);
    }
}
