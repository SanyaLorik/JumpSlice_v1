using Architecture_M;
using System;

public class DesktopInput : DesktopInputBase, IInputPlayer
{
    private PlayerInput _input;

    public event Action OnJumped;

    public override void Initialize()
    {
        _input = new PlayerInput();

        _input.Player.Jump.performed += OnJump;
    }

    public override void Dispose()
    {
        _input.Dispose();
    }

    public override void Enable()
    {
        _input.Enable();
    }

    public override void Disable()
    {
        _input.Disable();
    }

    private void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnJumped?.Invoke();
    }
}
