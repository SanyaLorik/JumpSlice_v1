using Architecture_M;
using DG.Tweening;
using SanyaBeerExtension;
using UnityEngine;
using UnityEngine.UI;

public class MiniTutorial : MonoBehaviour
{
    [Header("Finger")]
    [SerializeField] private DOTweenAnimationBase _finger;
    [SerializeField] private ParametrBase<CanvasGroup> _fingerImage;

    [Header("Key")]
    [SerializeField] private DOTweenAnimationBase _key;
    [SerializeField] private ParametrBase<CanvasGroup> _keyImage;

    public void StartTutorial()
    {
        StartAnimation(_finger, _fingerImage);
        StartAnimation(_key, _keyImage);
    }

    public void StopTutorial()
    {
        StopAnimation(_finger, _fingerImage);
        StopAnimation(_key, _keyImage);
    }

    private void StartAnimation(DOTweenAnimationBase finger, ParametrBase<CanvasGroup> fingerImage)
    {
        finger.ResetToInitialState();
        finger.Animate();

        fingerImage.Source.DOFade(1, float.MinValue);
    }

    private void StopAnimation(DOTweenAnimationBase finger, ParametrBase<CanvasGroup> fingerImage)
    {
        finger.Kill();

        fingerImage.Source
            .DOFade(0, fingerImage.Duration)
            .SetEase(fingerImage.Ease);
    }
}