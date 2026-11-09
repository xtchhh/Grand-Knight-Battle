using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouseBehavior : MonoBehaviour
{
    [Header("Input")]
    private Mouse mouse;
    private Camera cam;

    [Header("Mouse Properties")]
    public Vector3 mouseWorldPos;
    public bool isHitValid;
    public GameObject hitObject;

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
                isHitValid = true;
                var hitRadius = Instantiate(hitObject, mouseWorldPos, Quaternion.identity);
            }
        }
        else
        {
            isHitValid = false;
        }
    }
}
