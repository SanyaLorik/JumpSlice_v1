using Architecture_M;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainState : MonoBehaviour
{
    [Header("Window")]
    [SerializeField] private WindowBase _mainWindow;
    [SerializeField] private WindowBase _ingameWindow;

    [Header("Managment")]
    [SerializeField] private Button _button;

    [Inject] private WindowSwitcher _windowSwitcher;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnStartGame);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnStartGame);
    }

    private void OnStartGame()
    {
        StartGame().Forget();
    }

    private async UniTaskVoid StartGame()
    {
        await _windowSwitcher.Switch(_ingameWindow);
    }
}