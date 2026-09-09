using Architecture_M;
using System;

public class MobileInput : MobileInputBase<MobileInputView>, IInputPlayer
{
    public event Action OnJumped;

    public MobileInput(MobileInputView inputView) : base(inputView)
    {

    }

    public override void Initialize()
    {
        inputView.OnUpped += OnJump;
    }

    public override void Dispose()
    {
        inputView.OnUpped -= OnJump;
    }

    public override void Enable()
    {
        inputView.Enable();
    }

    public override void Disable()
    {
        inputView.Disable();
    }

    private void OnJump()
    {
        OnJumped?.Invoke();
    }
}