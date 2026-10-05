using Architecture_M;
using SanyaBeerExtension;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioPlayerManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private AudioSource _sourceButtonClick;

    private Button[] _buttons = null;

    [Inject] private IGameSave _gameSave;
    private GameSave _save;

    private void Awake()
    {
        _save = _gameSave.GetSave<GameSave>();
    }

    private void Start()
    {
        AddClickButton();
    }

    private void AddClickButton()
    {
        _buttons = FindObjectsByType<Button>(FindObjectsInactive.Include);
        _buttons.ForEach(i => i.onClick.AddListener(() => _sourceButtonClick.Play()));
    }
}