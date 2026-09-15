using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RecordState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _recoedWindow;

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
        await _recoedWindow.Show();
    }

    public override async UniTask Exit()
    {
        await _recoedWindow.Hide();
        await _menuState.Enter();
    }

    private void OnClose()
    {
        Exit().Forget();
    }
}