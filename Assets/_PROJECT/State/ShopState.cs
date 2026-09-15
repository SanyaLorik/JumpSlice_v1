using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _sjopWindow;

    [Header("Managment")]
    [SerializeField] private Button _close;

    [Header("States")]
    [SerializeField] private StateBase _menuState;

    private void OnEnable()
    {
        _close.onClick.AddListener(OnClose);
    }

    private void OnDisable()
    {
        _close.onClick.RemoveListener(OnClose);
    }

    public override async UniTask Enter()
    {
        await _sjopWindow.Show();
    }

    public override async UniTask Exit()
    {
        await _sjopWindow.Hide();
        await _menuState.Enter();
    }

    private void OnClose()
    {
        Exit().Forget();
    }
}