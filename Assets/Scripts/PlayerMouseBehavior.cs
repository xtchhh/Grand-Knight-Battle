using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseBehavior : MonoBehaviour
{
    [Header("Input")]
    private Mouse mouse;
    private Camera cam;

    [Header("Mouse Properties")]
    public static Vector3 mouseWorldPos;
    public static bool isInputValid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mouse = Mouse.current;
        cam = FindAnyObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePosition();
    }

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
}
