using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Freecam : MonoBehaviour
{
    [Header("Input")]
    public InputSystem_Actions input;
    private Vector2 move;

    [Header("Camera")]
    public Camera cam;
    public CinemachineInputAxisController camAxis;

    [Header("Movement")]
    private CharacterController characterController;
    private float moveSpeed = 20f;
    private float fastSpeed = 15f;

    //caching input
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        input = new InputSystem_Actions();
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
        DisableInput();
    }

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

    void DisableInput()
    {
        if (PlayerMouseState.currentMouseState == PlayerMouseState.MouseState.PressingRightClick)
        {
            camAxis.enabled = true;
            moveSpeed = 20;
        }
        else if (PlayerMouseState.currentMouseState == PlayerMouseState.MouseState.NotPressingRightClick)
        {
            camAxis.enabled = false;
            moveSpeed = 0f;
        }
    }
}