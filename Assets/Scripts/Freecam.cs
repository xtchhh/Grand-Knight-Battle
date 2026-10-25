using Unity.VisualScripting;
using UnityEngine;

public class Freecam : MonoBehaviour
{
    [Header("Input")]
    public InputSystem_Actions input;
    private Vector2 move;
    private Vector2 mouse;

    [Header("Camera")]
    public Camera cam;
    private float sens = 203f;

    [Header("Movement")]
    private CharacterController characterController;
    private float moveSpeed = 10f;
    private float fastSpeed = 15f;

    //caching input
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        input = new InputSystem_Actions();
        cam = GetComponentInChildren<Camera>();
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();    
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement(GetForwardDirection(), GetRightDirection());
        HandleCamera(GetCameraForward(), GetCameraRight());
    }

    #region Camera

    Vector3 GetCameraRight()
    {
        Vector3 cameraRight = cam.transform.right;
        float deltaRight = mouse.x;
        Vector3 cameraRightDirection = cameraRight * deltaRight * 20f;

        return cameraRightDirection;
    }

    Vector3 GetCameraForward()
    {
        float deltaForward = mouse.y;
        Vector3 cameraForward = cam.transform.forward;
        Vector3 cameraForwardDirection = cameraForward * deltaForward * 20f;

        return cameraForwardDirection;
    }

    void HandleCamera(Vector3 forward, Vector3 right)
    {
        mouse = input.Player.Look.ReadValue<Vector2>();

        Vector3 lookDirection = forward + right;

        cam.transform.localRotation = Quaternion.Euler(lookDirection * Time.deltaTime);
    }

    #endregion

    #region Movement
    Vector3 GetForwardDirection()
    {
        Vector3 cameraForward = cam.transform.forward;
        float forwardInput = move.y;
        Vector3 forwardDirection = (cameraForward * forwardInput).normalized;

        return forwardDirection;
    }

    Vector3 GetRightDirection()
    {
        Vector3 cameraRight = cam.transform.right;
        float rightInput = move.x;
        Vector3 rightDirection = (cameraRight * rightInput).normalized;

        return rightDirection;
    }

    void HandleMovement(Vector3 forward, Vector3 right)
    {
        move = input.Player.Move.ReadValue<Vector2>();
        Vector3 finalDirection = forward + right;
        characterController.Move(finalDirection * moveSpeed * Time.deltaTime);
    }

    #endregion
}