using Architecture_M;
using SanyaBeerExtension;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioPlayerManager : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private AudioSource _sourceButtonClick;

    [Header("Gameplay")]
    [SerializeField] private AudioSource[] _sourceGameplays;
    [SerializeField] private AudioClip _jumpClip;
    [SerializeField] private AudioClip _fallClip;
    [SerializeField] private AudioClip _platformAppearances;
    [SerializeField] private AudioClip _cubeCut;

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

    public void Jump()
    {
        Play(_jumpClip);
    }

    public void Fall()
    {
        Play(_fallClip);
    }

    public void PlatformAppearances()
    {
        Play(_platformAppearances);
    }

    public void CubeCut()
    {
        Play(_cubeCut);
    }

    private void Play(AudioClip audioClip)
    {
        AudioSource audioSource = _sourceGameplays.FirstOrDefault(i => i.isPlaying == false);
        if (audioSource == default)
            return;

        audioSource.clip = audioClip;
        audioSource.Play();
    }

    private void AddClickButton()
    {
        _buttons = FindObjectsByType<Button>(FindObjectsInactive.Include);
        _buttons.ForEach(i => i.onClick.AddListener(() => _sourceButtonClick.Play()));
    }
}