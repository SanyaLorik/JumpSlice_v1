using Cysharp.Threading.Tasks;
using SanyaBeerExtension;
using UnityEngine;

public class BackgroundParticle : MonoBehaviour
{
    [SerializeField] private ParticleSystem _psA;
    [SerializeField] private ParticleSystem _psB;
    [SerializeField] private Material _materialTemplate;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private PairedValue<float> _duration;

    private Material _matA;
    private Material _matB;
    private bool _useA = true;

    private void Start()
    {
        _matA = new Material(_materialTemplate);
        _matB = new Material(_materialTemplate);
        _psA.GetComponent<ParticleSystemRenderer>().material = _matA;
        _psB.GetComponent<ParticleSystemRenderer>().material = _matB;

        // стартуем только A
        _psB.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        ChangeAsync().Forget();
    }

    private async UniTaskVoid ChangeAsync()
    {
        while (!destroyCancellationToken.IsCancellationRequested)
        {
            // 1. Спавним новую текстуру в неактивной системе
            var nextPs = _useA ? _psB : _psA;
            var nextMat = _useA ? _matB : _matA;
            Sprite sprite = _sprites.GetRandomElement();
            nextMat.SetTexture("_BaseMap", sprite.texture);
            nextPs.Play();

            // 2. Старой системе запрещаем спавнить новые, но она доигрывает текущие
            var oldPs = _useA ? _psA : _psB;
            oldPs.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            _useA = !_useA;

            int duration = _duration.GetRandom().ToDelayMillisecond();
            await UniTask.Delay(duration, cancellationToken: destroyCancellationToken);
        }
    }
}