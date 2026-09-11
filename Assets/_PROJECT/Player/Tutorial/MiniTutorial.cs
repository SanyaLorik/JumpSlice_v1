using Architecture_M;
using DG.Tweening;
using SanyaBeerExtension;
using UnityEngine;
using UnityEngine.UI;

public class MiniTutorial : MonoBehaviour
{
    [SerializeField] private DOTweenAnimationBase _finger;
    [SerializeField] private ParametrBase<Image> _image;

    public void StartTutorial()
    {
        _finger.ResetToInitialState();
        _finger.Animate();

        _image.Source.DOFade(1, float.MinValue);
    }

    public void StopTutorial()
    {
        _finger.Kill();
        _image.Source
            .DOFade(0, _image.Duration)
            .SetEase(_image.Ease);
    }
}