using Architecture_M;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
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

    [Header("Gameplay")]
    [SerializeField] private MovementDirector _movementDirector;

    [Header("FX")]
    [SerializeField] private TrajectoryLine _trajectoryLine;

    [Inject] private WindowSwitcher _windowSwitcher;

    private void Start()
    {
        StartMainAsync().Forget();
    }

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
        StartGameAsync().Forget();
    }

    private async UniTaskVoid StartMainAsync()
    {
        _trajectoryLine.Hide();
    }

    private async UniTaskVoid StartGameAsync()
    {
        await _windowSwitcher.Switch(_ingameWindow);

        _movementDirector.StartDirection();

        await _trajectoryLine.ShowAnimationAsync();
    }
}