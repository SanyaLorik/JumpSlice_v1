using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

[Serializable]
public class BonusLarge
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _addScale = new(0.2f, 0f, 0.2f);
    [SerializeField] private Ease _ease = Ease.Unset;
    [SerializeField] private float _duration = 0.32f;

    public async UniTask Large()
    {
        Vector3 scale = _player.localScale + _addScale;

        await _player
            .DOScale(scale, _duration)
            .SetEase(_ease)
            .AsyncWaitForCompletion()
            .AsUniTask();
    }
}
