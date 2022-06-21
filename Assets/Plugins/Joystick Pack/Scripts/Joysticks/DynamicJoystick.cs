using UnityEngine;
using UnityEngine.InputSystem;

public class DynamicJoystick : Joystick
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

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
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

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}