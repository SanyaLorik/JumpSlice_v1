using Architecture_M;
using SanyaBeerExtension;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputView : MobileInputViewBase, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private GameObject _touchPlace;

    public event Action OnUpped;

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUpped?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void Enable()
    {
        _touchPlace.ActiveSelf();
    }

    public void Disable()
    {
        _touchPlace.DisactiveSelf();
    }
}