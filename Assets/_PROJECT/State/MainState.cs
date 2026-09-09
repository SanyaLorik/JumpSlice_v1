using Architecture_M;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MainState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _mainWindow;

    [Header("Managment")]
    [SerializeField] private Button _button;

    [Header("States")]
    [SerializeField] private StateBase _ingameState;

    [Header("Camera")]
    [SerializeField] private CameraMenu _menu;

    private void Start()
    {
        Enter().Forget();
    }
    private void OnEnable()
    {
        _button.onClick.AddListener(OnStartGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnStartGame);
    }

    public override async UniTask Enter()
    {
        StartMainAsync().Forget();
    }

    public override async UniTask Exit()
    {
        await _mainWindow.Hide();

        _menu.StopAnimation();

        await _ingameState.Enter();
    }

    private void OnStartGame()
    {
        Exit().Forget();
    }

    private async UniTaskVoid StartMainAsync()
    {
        _menu.StartAnimation();
    }
}