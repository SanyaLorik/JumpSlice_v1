using UnityEngine;

public class CameraState : MonoBehaviour
{
    [SerializeField] private CameraMenu _menu;

    public void CameraMenu()
    {
        _menu.StartAnimation();
    }
}