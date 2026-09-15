using Cysharp.Threading.Tasks;
using DG.Tweening;
using SanyaBeerExtension;
using UnityEngine;

public class PlayerRebuilder : MonoBehaviour
{
    [SerializeField] private Transform _initialPoint;
    [SerializeField] private Transform _playerParent;   
    [SerializeField] private ParametrBase<Transform> _slicedChild;   

    public async UniTask Rebuild()
    {
        _playerParent.SetPositionAndRotation(_initialPoint.position, _initialPoint.rotation);
        _slicedChild.Source.SetPositionAndRotation(Vector2.zero, Quaternion.identity);

        await _slicedChild.Source
            .DOScale(1, _slicedChild.Duration)
            .SetEase(_slicedChild.Ease)
            .AsyncWaitForCompletion();
    } 

    public void Zero()
    {
        _slicedChild.Source.localScale = Vector3.zero;
    }
}