using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMouseState : MonoBehaviour
{
    [Header("Input")]
    private Mouse mouse;

    [Header("Mouse Properties")]
    public static MouseState currentMouseState;

    public enum MouseState
    {
        PressingRightClick,
        NotPressingRightClick
    }

    void Awake()
    {
        mouse = Mouse.current;
    }

    void Update()
    {
        HandleCursorStateTransition();
        HandleMouseStateBehavior();
    }

    #region Mouse State
    void HandleCursorStateTransition()
    {
        bool pressingRightButton = mouse.rightButton.isPressed;

        if (pressingRightButton)
            currentMouseState = MouseState.PressingRightClick;
        else
            currentMouseState = MouseState.NotPressingRightClick;
    }

    void HandlePressingState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void HandleNotPressingState()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void HandleMouseStateBehavior()
    {
        switch (currentMouseState)
        {
            case MouseState.PressingRightClick:
                HandlePressingState();
                break;

            case MouseState.NotPressingRightClick:
                HandleNotPressingState();
                break;
        }
    }
    #endregion
}
