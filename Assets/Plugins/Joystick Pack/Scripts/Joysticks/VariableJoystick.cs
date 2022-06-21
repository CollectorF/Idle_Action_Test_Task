using UnityEngine;
using UnityEngine.InputSystem;

public enum JoystickType 
{ 
    Fixed, 
    Floating, 
    Dynamic 
}

public class VariableJoystick : Joystick
{
    public float MoveThreshold 
    {
        get 
        { 
            return moveThreshold;
        } 
        set
        {
            moveThreshold = Mathf.Abs(value);
        } 
    }

    [SerializeField]
    private float moveThreshold = 1;
    [SerializeField]
    private JoystickType joystickType = JoystickType.Fixed;

    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if(joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            background.gameObject.SetActive(true);
        }
        else
            background.gameObject.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(InputAction.CallbackContext callback)
    {
        if(joystickType != JoystickType.Fixed)
        {
#if UNITY_ANDROID || UNITY_IOS
            background.anchoredPosition = ScreenPointToAnchoredPosition(Touchscreen.current.primaryTouch.position.ReadValue());
#elif UNITY_STANDALONE
            background.anchoredPosition = ScreenPointToAnchoredPosition(Mouse.current.position.ReadValue());
#endif
            background.gameObject.SetActive(true);
        }
        base.OnPointerDown(callback);
    }

    public override void OnPointerUp(InputAction.CallbackContext callback)
    {
        if(joystickType != JoystickType.Fixed)
            background.gameObject.SetActive(false);

        base.OnPointerUp(callback);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}