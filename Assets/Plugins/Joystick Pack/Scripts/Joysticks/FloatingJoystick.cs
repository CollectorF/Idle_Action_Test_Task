using UnityEngine.InputSystem;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(InputAction.CallbackContext callback)
    {
#if UNITY_ANDROID || UNITY_IOS
        background.anchoredPosition = ScreenPointToAnchoredPosition(Touchscreen.current.primaryTouch.position.ReadValue());
#elif UNITY_STANDALONE
        background.anchoredPosition = ScreenPointToAnchoredPosition(Mouse.current.position.ReadValue());
#endif
        background.gameObject.SetActive(true);
        base.OnPointerDown(callback);
    }

    public override void OnPointerUp(InputAction.CallbackContext callback)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(callback);
    }
}