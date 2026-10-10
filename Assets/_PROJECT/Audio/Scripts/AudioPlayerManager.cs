using Architecture_M;
using SanyaBeerExtension;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioPlayerManager : MonoBehaviour
{
    public static AudioPlayerManager Instance { get; private set; }

    [Header("Button")]
    [SerializeField] private AudioSource _sourceButtonClick;

    [Header("Gameplay")]
    [SerializeField] private AudioSource[] _sourceGameplays;

    [SerializeField] private PaieredKeyValueReadOnly<AudioClipType, AudioClip>[] _clips;

    private Button[] _buttons = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AddClickButton();
    }

    public void Play(AudioClipType key)
    {
        AudioClip audioClip = _clips.FirstOrDefault(i => i.Key == key).Value;
        Play(audioClip);
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

public enum AudioClipType
{
    Jump,
    Fall,
    PlatformAppearance,
    CubeCute,
    Chance,
    GameOver,
    LargeBonus
}