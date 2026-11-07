using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class MouseBehavior : MonoBehaviour
{
    [Header("Input")]
    private Mouse mouse;
    private Camera cam;

    [Header("Mouse Properties")]
    public static MouseState currentMouseState;
    public static Vector3 mouseWorldPos;
    public static bool isInputValid;

    public enum MouseState
    {
        PressingRightClick,
        NotPressingRightClick
    }

    void Awake()
    {
        mouse = Mouse.current;
        Cursor.lockState = CursorLockMode.Confined;
        cam = FindAnyObjectByType<Camera>();
    }

    void Update()
    {
        HandleCursorStateTransition();
        HandleMouseStateBehavior();
        GetMousePosition();
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
    }

    void HandleNotPressingState()
    {
        Cursor.visible = true;
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

    #region Mouse Raycast
    void GetMousePosition()
    {
        if (mouse.leftButton.wasPressedThisFrame)
        {
            CheckInput();
        }
    }

    void CheckInput()
    {
        var mousePosition = mouse.position.ReadValue();
        Ray testRay = cam.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(testRay, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Clickable"))
            {
                mouseWorldPos = hit.point;
                isInputValid = true;
            }
        }
        else
        {
            isInputValid = false;
        }
    }
    #endregion
}
