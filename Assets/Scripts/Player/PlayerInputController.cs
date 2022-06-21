using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField]
    private InputAction clickInputAction;
    [SerializeField]
    private InputAction posInputAction;
    [SerializeField]
    private Joystick joystick;

    private void Awake()
    {
        clickInputAction.Enable();
        posInputAction.Enable();
        clickInputAction.started += joystick.OnPointerDown;
        clickInputAction.performed += joystick.OnPointerUp;
    }

    private void OnDisable()
    {
        clickInputAction.started -= joystick.OnPointerDown;
        clickInputAction.performed -= joystick.OnPointerUp;
        clickInputAction.Disable();
        posInputAction.Disable();
    }
}
