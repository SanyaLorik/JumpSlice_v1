using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomInput : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public event Action OnUp;

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnUp?.Invoke();
    }
}