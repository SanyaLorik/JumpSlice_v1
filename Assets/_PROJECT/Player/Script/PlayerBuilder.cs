using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanyaBeerExtension;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    [Header("Respawn")]
    [SerializeField] private Transform _initialPoint;
    [SerializeField] private Transform _playerParent;   
    [SerializeField] private ParametrBase<Transform> _slicedChild;

    private Rigidbody _rigidbody;

    public async UniTask RebuildAsync()
    {
        _playerParent.SetPositionAndRotation(_initialPoint.position, _initialPoint.rotation);
        _slicedChild.Source.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        await _slicedChild.Source
            .DOScale(1, _slicedChild.Duration)
            .SetEase(_slicedChild.Ease)
            .AsyncWaitForCompletion();
    } 

    public void Zero()
    {
        if (_rigidbody != null)
            Destroy(_rigidbody);

        _slicedChild.Source.localScale = Vector3.zero;
    }

    public void Lose()
    {
        _rigidbody = _playerParent.gameObject.AddComponent<Rigidbody>();
    }
}