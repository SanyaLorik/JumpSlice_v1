using System;
using Architecture_M;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

public class AudioVolumeManager : MonoBehaviour
{
    private const float MIN_VOLUME_DB = -80f;
    private const float MIN_LINEAR_VOLUME = 0.0001f;

    [Header("Ui")]
    [SerializeField] private Button _effectButton;
    [SerializeField] private Image _effectImage;
    [SerializeField] private Button _backgroundButton;
    [SerializeField] private Image _backgroundImage;

    [Header("State")]
    [SerializeField] private Sprite _onImage;
    [SerializeField] private Sprite _offImage;

    [Header("Mixer")]
    [SerializeField] private AudioMixer _effectMixer;
    [SerializeField] private AudioMixer _backgroundMixer;

    [Header("Mixer Parameters")]
    [SerializeField] private string _effectParameter = "Effects_Volume";
    [SerializeField] private string _backgroundParameter = "Background_Volume";

    [Header("Volume (linear 0..1)")]
    [SerializeField, Range(0f, 1f)] private float _maxEffectVolume = 1f;
    [SerializeField, Range(0f, 1f)] private float _maxBackgroundVolume = 1f;

    [Inject] private IGameSave _gameSave;
    private GameSave _save;

    private void Awake()
    {
        _save = _gameSave.GetSave<GameSave>();

        _effectButton.onClick.AddListener(ToggleEffect);
        _backgroundButton.onClick.AddListener(ToggleBackground);

        ChangeVolumeBySave();
    }

    private void OnDestroy()
    {
        if (_effectButton != null) _effectButton.onClick.RemoveListener(ToggleEffect);
        if (_backgroundButton != null) _backgroundButton.onClick.RemoveListener(ToggleBackground);
    }

    private void ChangeVolumeBySave()
    {
        ApplyEffectState(_save.IsEffectVolume, save: false);
        ApplyBackgroundState(_save.IsBackgroundVolume, save: false);

        ChangeVisual();
    }

    // -------- Effects --------
    public void ToggleEffect()
    {
        _save.IsEffectVolume = !_save.IsEffectVolume;
        ApplyEffectState(_save.IsEffectVolume);
        ChangeVisual();
    }

    private void ApplyEffectState(bool isOn, bool save = true)
    {
        float linear = isOn ? _maxEffectVolume : 0f;
        SetMixerVolume(_effectMixer, _effectParameter, linear);

        if (save)
        {
            _save.EffectVolume = linear;
            _gameSave.Save();
        }
    }

    // -------- Background --------
    public void ToggleBackground()
    {
        _save.IsBackgroundVolume = !_save.IsBackgroundVolume;
        ApplyBackgroundState(_save.IsBackgroundVolume);
        ChangeVisual();
    }

    private void ApplyBackgroundState(bool isOn, bool save = true)
    {
        float linear = isOn ? _maxBackgroundVolume : 0f;
        SetMixerVolume(_backgroundMixer, _backgroundParameter, linear);

        if (save)
        {
            _save.BackgroundVolume = linear;
            _gameSave.Save();
        }
    }

    // -------- Mixer --------
    private void SetMixerVolume(AudioMixer mixer, string parameter, float linearVolume)
    {
        if (mixer == null) return;

        // Защита от log(0) — при нулевой громкости ставим минимальный dB
        float dbVolume = linearVolume <= 0f
            ? MIN_VOLUME_DB
            : Mathf.Log10(Mathf.Max(linearVolume, MIN_LINEAR_VOLUME)) * 20f;

        mixer.SetFloat(parameter, dbVolume);
    }

    // -------- Visual --------
    private void ChangeVisual()
    {
        if (_effectImage != null)
            _effectImage.sprite = _save.IsEffectVolume ? _onImage : _offImage;

        if (_backgroundImage != null)
            _backgroundImage.sprite = _save.IsBackgroundVolume ? _onImage : _offImage;
    }
}
