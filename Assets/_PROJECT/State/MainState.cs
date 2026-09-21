using Architecture_M;
using Cysharp.Threading.Tasks;
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
    [SerializeField] private CameraPosition _cameraMenu;

    [Header("Player")]
    [SerializeField] private PlayerBuilder _playerBuilder;
    [SerializeField] private PlatfromGenerator _platfromGenerator;

    private MenuEnter _menuEnter = MenuEnter.First;

    private void Start()
    {
        EnterAsync().Forget();
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

    public override async UniTask EnterAsync()
    {
        if (_menuEnter == MenuEnter.First)
        {
            await _mainWindow.ShowAsync();
        }

        if (_menuEnter == MenuEnter.Ingame)
        {
            _playerBuilder.Zero();

            await _cameraMenu.ReturnCameraAsync();
            await _platfromGenerator.DestroyAllAsync();
            await _mainWindow.ShowAsync();

            await _playerBuilder.RebuildAsync();
        }

        if (_menuEnter == MenuEnter.Shop)
        {
            await _cameraMenu.ReturnCameraAsync();
            await _mainWindow.ShowAsync();
        }

        if (_menuEnter == MenuEnter.Other)
        {
            await _mainWindow.ShowAsync();
        }

        _cameraMenu.StartAnimation();
    }

    public override async UniTask ExitAsync()
    {
        await _mainWindow.HideAsync();

        _cameraMenu.StopAnimation();
    }

    private void OnStartGame()
    {
        _menuEnter = MenuEnter.Ingame;

        StartGameAsync().Forget();
    }

    private void OnSetting()
    {
        _menuEnter = MenuEnter.Other;
        SettingAsync().Forget();
    }

    private void OnRecord()
    {
        _menuEnter = MenuEnter.Other;
        RecordAsync().Forget();
    }

    private void OnShop()
    {
        _menuEnter = MenuEnter.Shop;
        ShopAsync().Forget();
    }

    private async UniTaskVoid StartGameAsync()
    {
        await ExitAsync();
        await _ingameState.EnterAsync();
    }

    private async UniTaskVoid SettingAsync()
    {
        await ExitAsync();
        await _settingState.EnterAsync();
    }

    private async UniTaskVoid RecordAsync()
    {
        await ExitAsync();
        await _recordState.EnterAsync();
    }

    private async UniTaskVoid ShopAsync()
    {
        await ExitAsync();
        await _shopState.EnterAsync();
    }

    private enum MenuEnter
    {
        First = 0,
        Ingame = 1,
        Shop = 2,
        Other
    }
}