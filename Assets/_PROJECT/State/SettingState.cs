using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SettingState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _settingWindow;

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

    public override async UniTask EnterAsync()
    {
        await _settingWindow.ShowAsync();
    }

    public override async UniTask ExitAsync()
    {
        await _settingWindow.HideAsync();
        await _menuState.EnterAsync();
    }

    private void OnClose()
    {
        ExitAsync().Forget();
    }
}