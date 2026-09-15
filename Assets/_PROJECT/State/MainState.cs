using Architecture_M;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainState : StateBase
{
    [Header("Window")]
    [SerializeField] private WindowBase _mainWindow;

    [Header("Managment")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _recordButton;
    [SerializeField] private Button _shopButton;

    [Header("States")]
    [SerializeField] private StateBase _ingameState;
    [SerializeField] private StateBase _settingState;
    [SerializeField] private StateBase _recordState;
    [SerializeField] private StateBase _shopState;

    [Header("Camera")]
    [SerializeField] private CameraMenu _menu;

    private void Start()
    {
        Enter().Forget();
    }
    private void OnEnable()
    {
        _startButton.onClick.AddListener(OnStartGame);
        _settingButton.onClick.AddListener(OnSetting);
        _recordButton.onClick.AddListener(OnRecord);
        _shopButton.onClick.AddListener(OnShop);
    }

    private void OnDisable()
    {
        _startButton.onClick.RemoveListener(OnStartGame);
        _settingButton.onClick.RemoveListener(OnSetting);
        _recordButton.onClick.RemoveListener(OnRecord);
        _shopButton.onClick.RemoveListener(OnShop);
    }

    public override async UniTask Enter()
    {
        StartMainAsync().Forget();

        await _mainWindow.Show();
    }

    public override async UniTask Exit()
    {
        await _mainWindow.Hide();

        _menu.StopAnimation();
    }

    private void OnStartGame()
    {
        StartGame().Forget();
    }

    private void OnSetting()
    {
        Setting().Forget();
    }

    private void OnRecord()
    {
        Record().Forget();
    }

    private void OnShop()
    {
        Shop().Forget();
    }

    private async UniTaskVoid StartGame()
    {
        await Exit();
        await _ingameState.Enter();
    }

    private async UniTaskVoid StartMainAsync()
    {
        _menu.StartAnimation();
    }

    private async UniTaskVoid Setting()
    {
        await Exit();
        await _settingState.Enter();
    }

    private async UniTaskVoid Record()
    {
        await Exit();
        await _recordState.Enter();
    }

    private async UniTaskVoid Shop()
    {
        await Exit();
        await _shopState.Enter();
    }
}